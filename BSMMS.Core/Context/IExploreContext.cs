using System;
using BSMMS.Core.Container;

namespace BSMMS.Core.Context
{
	public interface IExploreContext : IProcessStateContext
	{
		string UserName { get; }
		string Password { get; }
		string Keywords { get; }
		string CommentString { get; }
		bool Like { get; }
		bool Follow { get; }
		bool Comment { get; set; }
		bool Paging { get; }
		bool Unfollow { get; }
		bool UnfollowAll { get; }

		TimeoutRangeContainer TimeoutRange { get; }

		void UpdateCurrentImage(string imageUrl);
		void HandleException(Exception ex);
	}

	public interface IVerifyContext : ICommandContext
	{
		string MachineKey { get; set; }
		string LicenseKey { get; }
		bool LicenseVerified { get; set; }

		Action CloseAction { get; }
	}
}