using System.ComponentModel;
using BSMMS.Core.UI.View;
using BSMMS.Core.UI.ViewModel;

namespace BSMMS.Form.View
{
	/// <summary>
	/// Interaction logic for MainView.xaml
	/// </summary>
	public partial class MainView : BaseWindow
	{
		/// <summary>
		/// Gets or sets the view model.
		/// </summary>
		/// <value>
		/// The view model.
		/// </value>
		public override IBaseViewModel ViewModel { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="MainView"/> class.
		/// </summary>
		public MainView()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Attaches the context.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		public override void AttachContext(IBaseViewModel viewModel)
		{
			this.ViewModel = viewModel;
			this.ViewModel.Init();

			this.DataContext = this.ViewModel;
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Window.Closing" /> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data.</param>
		protected override void OnClosing(CancelEventArgs e)
		{
			var mainViewModel = this.ViewModel as MainViewModel;
			if (mainViewModel != null)
			{
				mainViewModel.InstagramVM.HandleCloseEvent();
			}

			base.OnClosing(e);
		}
	}
}
