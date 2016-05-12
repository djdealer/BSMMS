using System.Windows;
using BSMMS.Core.UI.ViewModel;

namespace BSMMS.Core.UI.View
{
	public abstract class BaseWindow : Window, IBaseWindow
	{
		public abstract IBaseViewModel ViewModel { get; set; }
		public abstract void AttachContext(IBaseViewModel viewModel);
	}

	public interface IBaseWindow
	{
		IBaseViewModel ViewModel { get; set; }
		void AttachContext(IBaseViewModel viewModel);
		void Show();
		bool? ShowDialog();
		void Close();
	}
}