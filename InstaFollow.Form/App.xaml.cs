using System.Windows;
using InstaFollow.Core.Extension;
using InstaFollow.Core.Factory;
using InstaFollow.Core.UI;
using InstaFollow.Core.UI.ViewModel;
using InstaFollow.Form.Service;
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
			if (LicenseService.Instance.IsValidRegistryLicenseCode())
			{
				windowService.CreateAndShowWindowModal<InstagramView, InstagramViewModel>();
			}
			else
			{
				var window = windowService.CreateAndShowWindowModal<LicenseView, LicenseViewModel>();

				if (window.ViewModel.LicenseVerified)
				{
					windowService.CreateAndShowWindowModal<InstagramView, InstagramViewModel>();
				}
			}

			this.Shutdown();
		}
	}
}
