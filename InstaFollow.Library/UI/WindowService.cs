using System;
using System.Windows;
using InstaFollow.Core.Container;

namespace InstaFollow.Core.UI
{
	public class WindowService : IWindowService
	{
		/// <summary>
		/// Shows the exception message box.
		/// </summary>
		/// <param name="ex">The exception.</param>
		/// <returns>A message box result.</returns>
		public MessageBoxResult ShowExceptionMessageBox(Exception ex)
		{
			return this.ShowMessageBox(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		/// <summary>
		/// Shows the message box.
		/// </summary>
		/// <param name="messageBoxText">The message box text.</param>
		/// <param name="caption">The caption.</param>
		/// <param name="button">The button.</param>
		/// <param name="icon">The icon.</param>
		/// <param name="defaultResult">The default result.</param>
		/// <param name="options">The options.</param>
		/// <returns>A message box result.</returns>
		public MessageBoxResult ShowMessageBox(string messageBoxText, string caption = "", MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information, MessageBoxResult defaultResult = MessageBoxResult.None, MessageBoxOptions options = MessageBoxOptions.None)
		{
			return MessageBox.Show(messageBoxText, caption, button, icon, defaultResult, options);
		}
	}
}