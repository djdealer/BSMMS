using System;
using System.Text.RegularExpressions;
using System.Threading;
using BSMMS.Core.Container;
using BSMMS.Core.Context;
using BSMMS.Core.Enum;
using BSMMS.Core.Exceptions;
using BSMMS.Core.Extension;
using log4net;
using Newtonsoft.Json;

namespace BSMMS.Core.Strategy
{
	public class InstagramUnfollowStrategy : BaseContextStrategy<IExploreContext>
	{
		private readonly ILog log = LogManager.GetLogger(typeof(InstagramUnfollowStrategy));

		private readonly IRandomizer rnd;

		private const string UserUri = @"https://www.instagram.com/{0}/";
		private const string QueryUri = @"https://www.instagram.com/query/";
		private const string UnfollowUri = @"https://www.instagram.com/web/friendships/{0}/unfollow/";
		private const string FollowingQueryPostString = @"q=ig_user(%0%){follows.first(10){count,page_info{end_cursor,has_next_page},nodes{id,is_verified,followed_by_viewer,requested_by_viewer,full_name,profile_pic_url,username}}}&ref=relationships::follow_list";
		private const string FollowingQueryPagePostString = @"q=ig_user(%0%){follows.after(%1%,10){count,page_info{end_cursor,has_next_page},nodes{id,is_verified,followed_by_viewer,requested_by_viewer,full_name,profile_pic_url,username}}}&ref=relationships::follow_list";

		private readonly IInstagramHttpContainer httpContainer;

		private string csrfToken, userId;
		private string referrer;

		public InstagramUnfollowStrategy(IExploreContext context, IInstagramHttpContainer httpContainer)
			: base(context)
		{
			this.httpContainer = httpContainer;
			this.rnd = Randomizer.Instance;
		}

		public void Unfollow(bool all)
		{
			try
			{
				ThreadDispatcher.Invoke(() => this.CurrentContext.ProcessState = ProcessState.Running);

				if (!this.httpContainer.InstagramLogin(this.CurrentContext.UserName, this.CurrentContext.Password))
				{
					throw new InstagramException("An error occured during login!");
				}

				var detailResponse = this.httpContainer.InstagramGet(string.Format(UserUri, this.CurrentContext.UserName));
				if (detailResponse == string.Empty)
				{
					throw new InstagramException("Could not get followed users from instagram!");
				}

				this.referrer = string.Format(UserUri, this.CurrentContext.UserName);
				this.csrfToken = Regex.Match(detailResponse.Replace(" ", string.Empty), "\"csrf_token\":\"(\\w+)\"").Groups[1].Value;

				var profileJson = Regex.Matches(detailResponse, "<script type=\"text/javascript\">window._sharedData =(.*?);</script>")[0].Groups[1].Value;
				dynamic dyn = JsonConvert.DeserializeObject(profileJson);
				dynamic dynPage = JsonConvert.DeserializeObject(dyn.entry_data.ProfilePage[0].ToString());

				this.userId = dynPage.user.id.ToString();

				var postData = FollowingQueryPostString.Replace("%0%", this.userId);

				var json = this.httpContainer.InstagramPost(QueryUri, this.csrfToken, string.Format(UserUri, this.CurrentContext.UserName), postData);
				if (json == string.Empty)
				{
					throw new InstagramException("Could not get followed users from instagram!");
				}
				
				dyn = JsonConvert.DeserializeObject(json);

				foreach (var node in dyn.follows.nodes)
				{
					var pic = node.profile_pic_url.ToString();
					
					ThreadDispatcher.Invoke(() => this.CurrentContext.UpdateCurrentImage(pic));

					if (this.ProcessStopped())
					{
						return;
					}

					Thread.Sleep(this.GetRandomTimeout());

					if (all || !this.Follows(node.username.ToString()))
					{
						this.log.Info("Unfollwing user: " + node.username.ToString());
						this.Unfollow(node.id.ToString());
					}
				}

				while (Convert.ToBoolean(dyn.follows.page_info.has_next_page) == true)
				{
					postData = FollowingQueryPagePostString.Replace("%0%", this.userId).Replace("%1%", dyn.follows.page_info.end_cursor.ToString());
					
					json = this.httpContainer.InstagramPost(QueryUri, this.csrfToken, string.Format(UserUri, this.CurrentContext.UserName), postData);
					if (json == string.Empty)
					{
						throw new InstagramException("Could not get followed users from instagram!");
					}

					dyn = JsonConvert.DeserializeObject(json);

					foreach (var node in dyn.follows.nodes)
					{
						var pic = node.profile_pic_url.ToString();

						ThreadDispatcher.Invoke(() => this.CurrentContext.UpdateCurrentImage(pic));

						if (this.ProcessStopped())
						{
							return;
						}

						Thread.Sleep(this.GetRandomTimeout());

						if (all || !this.Follows(node.username.ToString()))
						{
							this.log.Info("Unfollwing user: " + node.username.ToString());
							this.Unfollow(node.id.ToString());
						}
					}

					if (this.ProcessStopped())
					{
						return;
					}
				}

				this.log.Info("Finished.");
				ThreadDispatcher.Invoke(() => this.CurrentContext.ProcessState = ProcessState.Finished);
			}
			catch (Exception ex)
			{
				ThreadDispatcher.Invoke(() => this.CurrentContext.HandleException(ex));
			}
		}

		/// <summary>
		/// Checks, if the user follows us or not.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <returns></returns>
		/// <exception cref="InstagramException">Could not get followed users from instagram!</exception>
		private bool Follows(string userName)
		{
			var userResp = this.httpContainer.InstagramGet(string.Format(UserUri, userName));
			if (userResp == string.Empty)
			{
				throw new InstagramException("Could not get followed users from instagram!");
			}

			var userprofileJson = Regex.Matches(userResp, "<script type=\"text/javascript\">window._sharedData =(.*?);</script>")[0].Groups[1].Value;
			dynamic userDyn = JsonConvert.DeserializeObject(userprofileJson);
			dynamic userDynPage = JsonConvert.DeserializeObject(userDyn.entry_data.ProfilePage[0].ToString());
			var follows = Convert.ToBoolean(userDynPage.follows_viewer);

			this.log.Info("User " + userName + "follows: " + follows);

			return follows;
		}

		/// <summary>
		/// Unfollows the specified user by id.
		/// </summary>
		/// <param name="id">The identifier.</param>
		private void Unfollow(string id)
		{
			this.httpContainer.InstagramPost(string.Format(UnfollowUri, id), this.csrfToken, this.referrer);
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
		/// Checks, if the process was stopped or paused.
		/// Pause will hold on, stop will return true;
		/// </summary>
		/// <returns>True, if stop request was given.</returns>
		private bool ProcessStopped()
		{
			while (this.CurrentContext.ProcessState == ProcessState.Paused)
			{
				if (this.CurrentContext.ProcessState == ProcessState.Stopped)
				{
					this.log.Info("Stop request.");
					return true;
				}

				Thread.Sleep(100);
			}

			if (this.CurrentContext.ProcessState == ProcessState.Stopped)
			{
				this.log.Info("Stop request.");
				return true;
			}

			return false;
		}
	}
}