using System;
using System.Windows;
using BSMMS.Core.Factory;
using BSMMS.Core.UI.View;
using BSMMS.Core.UI.ViewModel;

namespace BSMMS.Form.Service
{
	public class WindowService : IWindowService
	{
		private static IWindowService instance;

		private WindowService() { }

		public static IWindowService Instance
		{
			get { return instance ?? (instance = new WindowService()); }
		}

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

		/// <summary>
		/// Creates and shows window modal.
		/// </summary>
		/// <typeparam name="T">The type of the window.</typeparam>
		/// <typeparam name="TVm">The type of the view model.</typeparam>
		public T CreateAndShowWindowModal<T, TVm>() 
			where T : IBaseWindow 
			where TVm : BaseViewModel
		{
			var vm = CoreFactory.Instance.CreateViewModel<TVm>(this, CoreFactory.Instance);
			var wnd = CoreFactory.Instance.CreateWindow<T>(vm);
			
			vm.CloseAction = new Action(wnd.Close);

			wnd.ShowDialog();

			return wnd;
		}

		/// <summary>
		/// Creates and shows window.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TVm">The type of the vm.</typeparam>
		public T CreateAndShowWindow<T, TVm>()
			where T : IBaseWindow
			where TVm : BaseViewModel
		{
			var vm = CoreFactory.Instance.CreateViewModel<TVm>(this, CoreFactory.Instance);
			var wnd = CoreFactory.Instance.CreateWindow<T>(vm);

			vm.CloseAction = new Action(wnd.Close);

			wnd.Show();

			return wnd;
		}
	}
}