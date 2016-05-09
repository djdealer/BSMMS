using System.ComponentModel;
using InstaFollow.Core.Factory;
using InstaFollow.Core.UI;
using InstaFollow.Core.UI.View;
using InstaFollow.Core.UI.ViewModel;

namespace InstaFollow.Form.View
{
	/// <summary>
	/// Interaction logic for InstagramView.xaml
	/// </summary>
	public partial class InstagramView : BaseWindow
	{
		public InstagramViewModel ViewModel { get; set; }

		public InstagramView()
		{
			this.InitializeComponent();
		}

		public override void AttachContext(IBaseViewModel viewModel)
		{
			this.ViewModel = viewModel as InstagramViewModel;
			this.ViewModel.Init();

			this.DataContext = this.ViewModel;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			this.ViewModel.HandleCloseEvent();

			base.OnClosing(e);
		}
	}
}
