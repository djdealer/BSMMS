using System;
using System.Windows;
using BSMMS.Core.UI.View;
using BSMMS.Core.UI.ViewModel;

namespace BSMMS.Core.Factory
{
	public interface IWindowService
	{
		MessageBoxResult ShowExceptionMessageBox(Exception ex);
		MessageBoxResult ShowMessageBox(string messageBoxText, string caption = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information, MessageBoxResult defaultResult = MessageBoxResult.None, MessageBoxOptions options = MessageBoxOptions.None);
		T CreateAndShowWindowModal<T, TVm>(BaseViewModel viewModel = null)
			where T : IBaseWindow
			where TVm : BaseViewModel;
		T CreateAndShowWindow<T, TVm>(BaseViewModel viewModel = null)
			where T : IBaseWindow
			where TVm : BaseViewModel;
	}
}