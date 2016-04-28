using System.Windows.Controls;
using InstaFollow.Core.Container;
using InstaFollow.Core.Factory;
using InstaFollow.Core.UI;
using InstaFollow.Scenario.ViewModel;

namespace InstaFollow.Form.View
{
	/// <summary>
	/// Interaction logic for MainView.xaml
	/// </summary>
	public partial class MainView : UserControl
	{
		public MainView()
		{
			this.InitializeComponent();
			
			var viewModel = CoreFactory.Instance.CreateViewModel<MainViewModel>(new WindowService(), CoreFactory.Instance);
			viewModel.Init();

			this.DataContext = viewModel;
		}
	}
}
