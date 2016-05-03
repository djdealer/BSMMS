using InstaFollow.Core.UI.View;
using InstaFollow.Core.UI.ViewModel;

namespace InstaFollow.Form.View
{
	/// <summary>
	/// Interaction logic for InfoView.xaml
	/// </summary>
	public partial class InfoView : BaseWindow, IInfoView
	{
		public InfoViewModel ViewModel { get; set; }

		public InfoView()
		{
			InitializeComponent();
		}

		public override void AttachContext(IBaseViewModel viewModel)
		{
			this.ViewModel = viewModel as InfoViewModel;
			this.ViewModel.Init();

			this.DataContext = this.ViewModel;
		}
	}
}
