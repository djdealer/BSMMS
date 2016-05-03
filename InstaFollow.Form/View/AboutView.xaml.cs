using System.Windows;
using InstaFollow.Core.UI.View;
using InstaFollow.Core.UI.ViewModel;

namespace InstaFollow.Form.View
{
	/// <summary>
	/// Interaction logic for AboutView.xaml
	/// </summary>
	public partial class AboutView : BaseWindow, IAboutView
	{
		public AboutViewModel ViewModel { get; set; }

		public AboutView()
		{
			this.InitializeComponent();
		}

		public override void AttachContext(IBaseViewModel viewModel)
		{
			this.ViewModel = viewModel as AboutViewModel;
			this.ViewModel.Init();

			this.DataContext = this.ViewModel;
		}
	}
}
