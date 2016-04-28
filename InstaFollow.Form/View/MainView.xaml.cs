using System.Windows.Controls;
using InstaFollow.Core.Container;
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
			
			this.DataContext = new MainViewModel(new MessageBoxService());
		}
	}
}
