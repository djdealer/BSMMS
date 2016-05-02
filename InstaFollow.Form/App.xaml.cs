using System.Windows;
using InstaFollow.Core.Factory;
using InstaFollow.Core.UI;
using InstaFollow.Core.UI.ViewModel;
using InstaFollow.Form.View;

namespace InstaFollow.Form
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void OnStartUp(object sender, StartupEventArgs e)
		{
			this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
			var windowService = WindowService.Instance;

			// todo read licence file

			var licensed = false; // TODO
			if (licensed)
			{
				windowService.CreateAndShowWindowModal<MainWindow, MainViewModel>();
			}
			else
			{
				var window = windowService.CreateAndShowWindowModal<LicenseWindow, LicenseViewModel>();

				if (window.ViewModel.LicenseVerified)
				{
					windowService.CreateAndShowWindowModal<MainWindow, MainViewModel>();
				}
			}

			this.Shutdown();
		}
	}
}
