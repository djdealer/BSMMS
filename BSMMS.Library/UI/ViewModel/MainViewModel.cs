using System.Windows.Input;
using BSMMS.Core.Context;
using BSMMS.Core.Extension;
using BSMMS.Core.UI.Command;

namespace BSMMS.Core.UI.ViewModel
{
	public class MainViewModel : BaseViewModel, IInfoContext
	{
		/// <summary>
		/// Initializes this instance.
		/// </summary>
		public override void Init()
		{
			ThreadDispatcher.Initialize();
			this.InfoCommand = this.CoreFactory.CreateContextCommand<InfoCommand, IInfoContext>(this);
		}

		/// <summary>
		/// Gets or sets the information command.
		/// </summary>
		/// <value>
		/// The information command.
		/// </value>
		public ICommand InfoCommand { get; set; }

		/// <summary>
		/// Gets the status text.
		/// </summary>
		/// <value>
		/// The status text.
		/// </value>
		public string StatusText { get { return "BRANOVA Social Media Marketing Suite."; } }
	}
}