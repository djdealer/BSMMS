using System;
using System.Windows;
using InstaFollow.Core.UI.View;
using InstaFollow.Core.UI.ViewModel;

namespace InstaFollow.Core.UI
{
	public interface IWindowService
	{
		MessageBoxResult ShowExceptionMessageBox(Exception ex);
		MessageBoxResult ShowMessageBox(string messageBoxText, string caption = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information, MessageBoxResult defaultResult = MessageBoxResult.None, MessageBoxOptions options = MessageBoxOptions.None);
		T CreateAndShowWindowModal<T, TVm>() where T : BaseWindow where TVm : BaseViewModel;
		T CreateAndShowWindow<T, TVm>()where T : BaseWindow where TVm : BaseViewModel;
	}
}