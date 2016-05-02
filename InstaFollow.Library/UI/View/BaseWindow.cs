using System.Windows;
using InstaFollow.Core.UI.ViewModel;

namespace InstaFollow.Core.UI.View
{
	public abstract class BaseWindow : Window
	{
		public abstract void AttachContext(IBaseViewModel viewModel);
	}
}