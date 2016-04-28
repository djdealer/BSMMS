using System;
using System.Windows;

namespace InstaFollow.Core.UI
{
	public interface IWindowService
	{
		MessageBoxResult ShowExceptionMessageBox(Exception ex);
		MessageBoxResult ShowMessageBox(string messageBoxText, string caption = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information, MessageBoxResult defaultResult = MessageBoxResult.None, MessageBoxOptions options = MessageBoxOptions.None);
	}
}