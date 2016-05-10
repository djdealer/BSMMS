using BSMMS.Core.UI.View;
using BSMMS.Core.UI.ViewModel;

namespace BSMMS.Form.View
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
