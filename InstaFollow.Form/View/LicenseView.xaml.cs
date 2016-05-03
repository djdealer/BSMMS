using System.Windows;
using InstaFollow.Core.UI.View;
using InstaFollow.Core.UI.ViewModel;

namespace InstaFollow.Form.View
{
	/// <summary>
	/// Interaction logic for LicenseView.xaml
	/// </summary>
	public partial class LicenseView : BaseWindow
	{
		public LicenseViewModel ViewModel { get; set; }

		public LicenseView()
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
