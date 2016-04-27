using InstaFollow.Library.Container;

namespace InstaFollow.Scenario.Context
{
	public interface IStartProcessContext : IProcessStateContext
	{
		string UserName { get; }
		string Password { get; }
		string Keywords { get; }
		string CommentString { get; }
		bool Like { get; }
		bool Follow { get; }
		bool Comment { get; set; }
		bool Paging { get; }

		TimeoutRangeContainer TimeoutRange { get; }

		void UpdateCurrentImage(string imageUrl);
	}
}