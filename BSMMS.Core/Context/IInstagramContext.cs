using BSMMS.Core.Factory;
using BSMMS.Core.UI.ViewModel;

namespace BSMMS.Core.Context
{
	public interface IInstagramContext : ICommandContext
	{
		IWindowService WindowService { get; }
		InstagramViewModel InstagramVM { get; set; }
		void InstagramNotifyHandler();
	}
}