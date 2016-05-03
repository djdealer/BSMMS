using InstaFollow.Core.UI.ViewModel;

namespace InstaFollow.Core.UI.View
{
	public interface IBaseWindow
	{
		void AttachContext(IBaseViewModel viewModel);
		void Show();
		bool? ShowDialog();
		void Close();
	}
}