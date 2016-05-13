using System;
using BSMMS.Core.Container;
using BSMMS.Core.Context;
using BSMMS.Core.Enum;
using BSMMS.Core.Exceptions;
using BSMMS.Core.Extension;

namespace BSMMS.Core.Strategy
{
	public class UnfollowStrategy : BaseContextStrategy<IExploreContext>
	{
		private const string QueryUri = @"https://www.instagram.com/query/";
		// todo %0% user id
		private const string FollowingQueryPostString = @"q=ig_user(%0%){follows.first(10){count,page_info{end_cursor,has_next_page},nodes{id,is_verified,followed_by_viewer,requested_by_viewer,full_name,profile_pic_url,username}}}&ref=relationships::follow_list";

		private readonly IInstagramHttpContainer httpContainer;

		public UnfollowStrategy(IExploreContext context, IInstagramHttpContainer httpContainer)
			: base(context)
		{
			this.httpContainer = httpContainer;
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

				if (all)
				{
					this.UnfollowAll();
				}
				else
				{
					this.UnfollowNotFollowing();
				}
			}
			catch (Exception ex)
			{
				ThreadDispatcher.Invoke(() => this.CurrentContext.HandleException(ex));
			}
		}

		private void UnfollowAll()
		{
			
		}

		private void UnfollowNotFollowing()
		{
			
		}
	}
}