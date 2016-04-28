using System;
using System.Text.RegularExpressions;
using System.Threading;
using InstaFollow.Core.Container;
using InstaFollow.Core.Context;
using InstaFollow.Core.Enum;
using InstaFollow.Core.Exceptions;
using InstaFollow.Core.Extension;
using log4net;
using Newtonsoft.Json;

namespace InstaFollow.Scenario.Strategy
{
	public class ExploreStrategy : BaseContextStrategy<IExploreContext>
	{
		#region private members

		private readonly ILog log = LogManager.GetLogger(typeof(ExploreStrategy));

		private const string ExploreUri = @"https://www.instagram.com/explore/tags/{0}/";
		private const string DetailUri = @"https://www.instagram.com/p/{0}/";
		private const string PageQueryString = @"https://www.instagram.com/query/";
		private const string LikeUri = @"https://www.instagram.com/web/likes/{0}/like/";
		private const string FollowUri = @"https://www.instagram.com/web/friendships/{0}/follow/";
		private const string CommentUri = @"https://www.instagram.com/web/comments/{0}/add/";
		private const string PageQueryPostString = @"q=ig_hashtag(%0%){media.after(%1%,12){count,nodes{caption,code,comments{count},date,dimensions{height,width},display_src,id,is_video,likes{count},owner{id},thumbnail_src},page_info}}&ref=tags::show";

		private readonly IRandomizer rnd;

		private string ImageCode { get; set; }
		private string Comment { get; set; }
		private string AuthorId { get; set; }
		private string ImageId { get; set; }
		private string CsrfToken { get; set; }
		private string Referrer { get; set; }

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="ExploreStrategy"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		public ExploreStrategy(IExploreContext context) : base(context)
		{
			this.rnd = Randomizer.Instance;
		}

		/// <summary>
		/// Main strategy function. Explores instagram by keyword, likes, comments, followes
		/// based on settings coming from UI.
		/// </summary>
		public void Explore()
		{
			try
			{
				ThreadDispatcher.Invoke(() => this.CurrentContext.ProcessState = ProcessState.Running);

				if (!InstagramInstagramHttpContainer.Instance.InstagramLogin(this.CurrentContext.UserName, this.CurrentContext.Password))
				{
					throw new InstagramException("An error occured during login!");
				}

				this.ExploreKeywords();
			}
			catch (Exception ex)
			{
				ThreadDispatcher.Invoke(() => this.CurrentContext.HandleException(ex));	
			}
		}

		private void ExploreKeywords()
		{
			foreach (var keyword in this.CurrentContext.Keywords.Split('|'))
			{
				this.log.Info("Working on keyword: " + keyword);
				if (this.CurrentContext.ProcessState == ProcessState.Stopped)
				{
					this.log.Info("Stop request.");
					return;
				}

				var exploreResponse = InstagramInstagramHttpContainer.Instance.InstagramGet(string.Format(ExploreUri, keyword));
				if (exploreResponse == string.Empty)
				{
					continue;
				}

				var mediaJson =
					Regex.Matches(exploreResponse, "<script type=\"text/javascript\">window._sharedData =(.*?);</script>")[0].Groups[1]
						.Value;
				var csrfToken = Regex.Match(exploreResponse, "\"csrf_token\":\"(\\w+)\"").Groups[1].Value;

				dynamic dyn = JsonConvert.DeserializeObject(mediaJson);

				foreach (var node in dyn.entry_data.TagPage[0].tag.media.nodes)
				{
					this.log.Info("Working on image: " + node.code.ToString());

					if (this.CurrentContext.ProcessState == ProcessState.Stopped)
					{
						this.log.Info("Stop request.");
						return;
					}

					this.SetNewImage(node);

					this.ImageCode = node.code.ToString();
					this.GetDetails();
					Thread.Sleep(this.GetRandomTimeout());
				}

				var postData = PageQueryPostString.Replace("%0%", this.CurrentContext.Keywords)
					.Replace("%1%", dyn.entry_data.TagPage[0].tag.media.page_info.end_cursor.ToString());

				var json = InstagramInstagramHttpContainer.Instance.InstagramPost(PageQueryString, csrfToken,
					string.Format(ExploreUri, this.CurrentContext.Keywords), postData);
				dyn = JsonConvert.DeserializeObject(json);

				if (this.CurrentContext.Paging)
				{
					while (Convert.ToBoolean(dyn.media.page_info.has_next_page) == true)
					{
						foreach (var node in dyn.media.nodes)
						{
							this.log.Info("Working on image: " + node.code.ToString());

							if (this.CurrentContext.ProcessState == ProcessState.Stopped)
							{
								this.log.Info("Stop request.");
								return;
							}

							this.SetNewImage(node);

							this.ImageCode = node.code.ToString();
							this.GetDetails();
							Thread.Sleep(this.GetRandomTimeout());
						}

						if (this.CurrentContext.ProcessState == ProcessState.Stopped)
						{
							this.log.Info("Stop request.");
							return;
						}

						Thread.Sleep(this.GetRandomTimeout());

						postData = PageQueryPostString.Replace("%0%", this.CurrentContext.Keywords)
							.Replace("%1%", dyn.media.page_info.end_cursor.ToString());

						json = InstagramInstagramHttpContainer.Instance.InstagramPost(PageQueryString, csrfToken,
							string.Format(ExploreUri, this.CurrentContext.Keywords), postData);
						dyn = JsonConvert.DeserializeObject(json);
					}
				}
			}

			ThreadDispatcher.Invoke(() => this.CurrentContext.ProcessState = ProcessState.Finished);
		}

		/// <summary>
		/// Gets the details for an instagram image. Likes, comments, followes
		/// based on settings coming from UI.
		/// </summary>
		private void GetDetails()
		{
			try
			{
				var detailResponse = InstagramInstagramHttpContainer.Instance.InstagramGet(string.Format(DetailUri, this.ImageCode));
				if (detailResponse == string.Empty)
				{
					return;
				}

				var mediaJson = Regex.Matches(detailResponse, "<script type=\"text/javascript\">window._sharedData =(.*?);</script>")[0].Groups[1].Value;
				this.CsrfToken = Regex.Match(detailResponse, "\"csrf_token\":\"(\\w+)\"").Groups[1].Value;

				dynamic dyn = JsonConvert.DeserializeObject(mediaJson);
				dynamic dynMedia = JsonConvert.DeserializeObject(dyn.entry_data.PostPage[0].ToString());

				this.ImageId = dynMedia.media.id.ToString();
				this.AuthorId = dynMedia.media.owner.id.ToString();
				this.Referrer = string.Format(DetailUri, this.ImageCode);
				var followed = Convert.ToBoolean(dynMedia.media.owner.followed_by_viewer);
				var requested = Convert.ToBoolean(dynMedia.media.owner.requested_by_viewer);
				var liked = Convert.ToBoolean(dynMedia.media.likes.viewer_has_liked);
				var commented = this.CheckAlreadyCommented(dynMedia.media.comments);

				var random = this.rnd.Generate(0, 1000);

				if (this.CurrentContext.Follow && ((random > 0 && random < 333) || (random > 666)) && !followed && !requested)
				{
					this.log.Info("Following user id: " + this.AuthorId);
					this.FollowItemAuthor();
					Thread.Sleep(this.GetRandomTimeout());
				}

				if (this.CurrentContext.Like && !liked)
				{
					this.log.Info("Liking image id: " + this.ImageId);
					this.LikeItem();
					Thread.Sleep(this.GetRandomTimeout());
				}

				if (this.CurrentContext.Comment && random > 333 && random < 666 && !commented)
				{
					this.Comment = new TextSpinner().Spin(this.CurrentContext.CommentString);

					this.log.Info("Commenting image id: " + this.ImageId + " with text: '" + this.Comment + "'");
					this.CommentItem();
					Thread.Sleep(this.GetRandomTimeout());
				}	
			}
			catch (FormatException fex)
			{
				this.log.Error(fex.Message);
				throw;
			}
		}

		/// <summary>
		/// Follows the item author.
		/// </summary>
		private void FollowItemAuthor()
		{
			InstagramInstagramHttpContainer.Instance.InstagramPost(string.Format(FollowUri, this.AuthorId), this.CsrfToken, this.Referrer);
		}

		/// <summary>
		/// Likes the item.
		/// </summary>
		private void LikeItem()
		{
			InstagramInstagramHttpContainer.Instance.InstagramPost(string.Format(LikeUri, this.ImageId), this.CsrfToken, this.Referrer);
		}

		/// <summary>
		/// CommentString the item.
		/// </summary>
		private void CommentItem()
		{
			try
			{
				var postData = "comment_text=" + this.Comment;
				InstagramInstagramHttpContainer.Instance.InstagramPost(string.Format(CommentUri, this.ImageId), this.CsrfToken,
					this.Referrer, postData, true);
			}
			catch (InstagramCommentException ex)
			{
				ThreadDispatcher.Invoke(() => this.CurrentContext.Comment = false);
				this.log.Error(ex.Message);
			}
		}

		/// <summary>
		/// Gets a random timeout value.
		/// Range of the value is read out of the current context.
		/// </summary>
		/// <returns>An integer containing milliseconds.</returns>
		private int GetRandomTimeout()
		{
			return this.rnd.Generate(this.CurrentContext.TimeoutRange.MinTimeout * 1000,
				this.CurrentContext.TimeoutRange.MaxTimeout * 1000);
		}

		/// <summary>
		/// Checks if the current image was already commented.
		/// </summary>
		/// <param name="commentNode">The comment node.</param>
		/// <returns>True if user already commented this image, false otherwise.</returns>
		private bool CheckAlreadyCommented(dynamic commentNode)
		{
			// TODO analyse, if there is a paging. //Convert.ToBoolean(commentNode.page_info.has_next_page)
			foreach (var node in commentNode.nodes)
			{
				if (node.user.username == this.CurrentContext.UserName)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Sets the current image in main window (thread safe).
		/// </summary>
		/// <param name="node">The node.</param>
		private void SetNewImage(dynamic node)
		{
			ThreadDispatcher.Invoke(() => this.CurrentContext.UpdateCurrentImage(node.display_src.ToString().Replace("\\", string.Empty)));
		}
	}
}