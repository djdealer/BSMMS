using InstaFollow.Core.Container;
using InstaFollow.Core.Factory;
using InstaFollow.Core.UI;
using InstaFollow.Core.UI.ViewModel;
using MvvmFoundation.Wpf;

namespace InstaFollow.Scenario.ViewModel
{
	public abstract class BaseViewModel : ObservableObject, IBaseViewModel
	{
		public IWindowService WindowService { get; set; }
		public ICoreFactory CoreFactory { get; set; }

		protected BaseViewModel() { }
	}
}