using System.Windows;

namespace InstaFollow.Core.Container
{
	public class MessageBoxService : IMessageBoxService
	{
		public MessageBoxResult Show(string messageBoxText, string caption = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information, MessageBoxResult defaultResult = MessageBoxResult.None, MessageBoxOptions options = MessageBoxOptions.None)
		{
			return MessageBox.Show(messageBoxText, caption, button, icon, defaultResult, options);
		}
	}
}