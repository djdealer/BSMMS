using System.ComponentModel;
using System.Windows;
using InstaFollow.Core.Factory;
using InstaFollow.Core.UI;
using InstaFollow.Scenario.ViewModel;

namespace InstaFollow.Form.View
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly MainViewModel vm;
		public MainWindow()
		{
			this.InitializeComponent();

			this.vm = CoreFactory.Instance.CreateViewModel<MainViewModel>(new WindowService(), CoreFactory.Instance);
			this.vm.Init();

			this.DataContext = this.vm;
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			this.vm.HandleCloseEvent();

			base.OnClosing(e);
		}
	}
}
