using System.Windows;
using InstaFollow.Core.UI.View;
using InstaFollow.Core.UI.ViewModel;

namespace InstaFollow.Form.View
{
	/// <summary>
	/// Interaction logic for LicenseWindow.xaml
	/// </summary>
	public partial class LicenseWindow : BaseWindow
	{
		public LicenseViewModel ViewModel { get; set; }

		public LicenseWindow()
		{
			InitializeComponent();
		}

		public override void AttachContext(IBaseViewModel viewModel)
		{
			this.ViewModel = viewModel as LicenseViewModel;
			this.ViewModel.Init();

			this.DataContext = this.ViewModel;
		}
	}
}
