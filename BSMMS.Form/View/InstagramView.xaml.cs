using System.ComponentModel;
using BSMMS.Core.UI.View;
using BSMMS.Core.Factory;
using BSMMS.Core.UI.ViewModel;

namespace BSMMS.Form.View
{
	/// <summary>
	/// Interaction logic for InstagramView.xaml
	/// </summary>
	public partial class InstagramView : BaseWindow, IInstagramView
	{
		public override IBaseViewModel ViewModel { get; set; }

		public InstagramView()
		{
			this.InitializeComponent();
		}

		public override void AttachContext(IBaseViewModel viewModel)
		{
			this.ViewModel = viewModel;
			this.ViewModel.Init();

			this.DataContext = this.ViewModel;
		}
	}
}
