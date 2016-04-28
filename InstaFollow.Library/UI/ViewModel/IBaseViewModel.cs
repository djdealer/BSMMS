using InstaFollow.Core.Container;
using InstaFollow.Core.Factory;

namespace InstaFollow.Core.UI.ViewModel
{
	public interface IBaseViewModel
	{
		IWindowService WindowService { get; set; }
		ICoreFactory CoreFactory { get; set; }
	}
}