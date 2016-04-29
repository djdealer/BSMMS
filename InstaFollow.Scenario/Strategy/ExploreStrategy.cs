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
		private readonly IInstagramHttpContainer httpContainer;

		private string imageCode, comment, authorId, imageId, csrfToken, referrer;

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="ExploreStrategy" /> class.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="httpContainer">The HTTP container.</param>
		public ExploreStrategy(IExploreContext context, IInstagramHttpContainer httpContainer) : base(context)
		{
			this.rnd = Randomizer.Instance;
			this.httpContainer = httpContainer;
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

				if (!this.httpContainer.InstagramLogin(this.CurrentContext.UserName, this.CurrentContext.Password))
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

		/// <summary>
		/// Explores the keywords.
		/// </summary>
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

				var exploreResponse = this.httpContainer.InstagramGet(string.Format(ExploreUri, keyword));
				if (exploreResponse == string.Empty)
				{
					continue;
				}

				var mediaJson =
					Regex.Matches(exploreResponse, "<script type=\"text/javascript\">window._sharedData =(.*?);</script>")[0].Groups[1]
						.Value;
				var keywordCsrf = Regex.Match(exploreResponse, "\"csrf_token\":\"(\\w+)\"").Groups[1].Value;

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

					this.imageCode = node.code.ToString();
					this.GetDetails();
					Thread.Sleep(this.GetRandomTimeout());
				}

				if (this.CurrentContext.Paging && Convert.ToBoolean(dyn.entry_data.TagPage[0].tag.media.page_info.has_next_page))
				{
					var postData = PageQueryPostString.Replace("%0%", keyword)
					.Replace("%1%", dyn.entry_data.TagPage[0].tag.media.page_info.end_cursor.ToString());

					var json = this.httpContainer.InstagramPost(PageQueryString, keywordCsrf,
						string.Format(ExploreUri, keyword), postData);
					dyn = JsonConvert.DeserializeObject(json);

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

							this.imageCode = node.code.ToString();
							this.GetDetails();
							Thread.Sleep(this.GetRandomTimeout());
						}

						if (this.CurrentContext.ProcessState == ProcessState.Stopped)
						{
							this.log.Info("Stop request.");
							return;
						}

						Thread.Sleep(this.GetRandomTimeout());

						postData = PageQueryPostString.Replace("%0%", keyword)
							.Replace("%1%", dyn.media.page_info.end_cursor.ToString());

						json = this.httpContainer.InstagramPost(PageQueryString, keywordCsrf,
							string.Format(ExploreUri, keyword), postData);
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
				var detailResponse = this.httpContainer.InstagramGet(string.Format(DetailUri, this.imageCode));
				if (detailResponse == string.Empty)
				{
					return;
				}

				var mediaJson = Regex.Matches(detailResponse, "<script type=\"text/javascript\">window._sharedData =(.*?);</script>")[0].Groups[1].Value;
				this.csrfToken = Regex.Match(detailResponse, "\"csrf_token\":\"(\\w+)\"").Groups[1].Value;

				dynamic dyn = JsonConvert.DeserializeObject(mediaJson);
				dynamic dynMedia = JsonConvert.DeserializeObject(dyn.entry_data.PostPage[0].ToString());

				this.imageId = dynMedia.media.id.ToString();
				this.authorId = dynMedia.media.owner.id.ToString();
				this.referrer = string.Format(DetailUri, this.imageCode);
				var followed = Convert.ToBoolean(dynMedia.media.owner.followed_by_viewer);
				var requested = Convert.ToBoolean(dynMedia.media.owner.requested_by_viewer);
				var liked = Convert.ToBoolean(dynMedia.media.likes.viewer_has_liked);
				var commented = this.CheckAlreadyCommented(dynMedia.media.comments);

				var random = this.rnd.Generate(0, 1000);

				if (this.CurrentContext.Follow && ((random > 0 && random < 333) || (random > 666)) && !followed && !requested)
				{
					Thread.Sleep(this.GetRandomTimeout());
					this.log.Info("Following user id: " + this.authorId);
					this.FollowItemAuthor();
				}

				if (this.CurrentContext.Like && random > 666 && !liked)
				{
					Thread.Sleep(this.GetRandomTimeout());
					this.log.Info("Liking image id: " + this.imageId);
					this.LikeItem();
				}

				if (this.CurrentContext.Comment && random > 333 && random < 666 && !commented)
				{
					Thread.Sleep(this.GetRandomTimeout());

					this.comment = new TextSpinner().Spin(this.CurrentContext.CommentString);

					this.log.Info("Commenting image id: " + this.imageId + " with text: '" + this.comment + "'");
					this.CommentItem();
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
			this.httpContainer.InstagramPost(string.Format(FollowUri, this.authorId), this.csrfToken, this.referrer);
		}

		/// <summary>
		/// Likes the item.
		/// </summary>
		private void LikeItem()
		{
			this.httpContainer.InstagramPost(string.Format(LikeUri, this.imageId), this.csrfToken, this.referrer);
		}

		/// <summary>
		/// CommentString the item.
		/// </summary>
		private void CommentItem()
		{
			try
			{
				var postData = "comment_text=" + this.comment;
				this.httpContainer.InstagramPost(string.Format(CommentUri, this.imageId), this.csrfToken,
					this.referrer, postData, true);
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