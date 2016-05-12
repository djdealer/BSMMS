using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BSMMS.Core.UI.View;
using BSMMS.Core.UI.ViewModel;

namespace BSMMS.Form.View
{
	/// <summary>
	/// Interaction logic for MainView.xaml
	/// </summary>
	public partial class MainView : BaseWindow
	{
		public MainViewModel ViewModel { get; set; }

		public MainView()
		{
			this.InitializeComponent();
		}

		public override void AttachContext(IBaseViewModel viewModel)
		{
			this.ViewModel = viewModel as MainViewModel;
			this.ViewModel.Init();

			this.DataContext = this.ViewModel;
		}
	}
}
