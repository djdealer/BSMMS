using System.Windows.Input;
using BSMMS.Core.Context;
using BSMMS.Core.Enum;
using BSMMS.Core.Extension;
using BSMMS.Core.UI.Command;

namespace BSMMS.Core.UI.ViewModel
{
	public class MainViewModel : BaseViewModel, IInfoContext, IInstagramContext
	{
		/// <summary>
		/// Initializes this instance.
		/// </summary>
		public override void Init()
		{
			ThreadDispatcher.Initialize();
			this.InfoCommand = this.CoreFactory.CreateContextCommand<InfoCommand, IInfoContext>(this);
			this.InstagramWindowCommand = this.CoreFactory.CreateContextCommand<InstagramWindowCommand, IInstagramContext>(this);
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

		#region instagram

		/// <summary>
		/// Gets or sets the instagram window command.
		/// </summary>
		/// <value>
		/// The instagram window command.
		/// </value>
		public ICommand InstagramWindowCommand { get; set; }

		/// <summary>
		/// Gets or sets the instagram view model.
		/// </summary>
		/// <value>
		/// The instagram view model.
		/// </value>
		public InstagramViewModel InstagramVM { get; set; }

		/// <summary>
		/// Gets the instagram progress bulb.
		/// </summary>
		/// <value>
		/// The instagram progress bulb.
		/// </value>
		public string InstagramProgressBulb
		{
			get
			{
				return this.InstagramProcessRunning && this.InstagramVM != null && this.InstagramVM.ProcessState != ProcessState.Paused 
				  ? @"..\Images\green_light.png"
				  : this.InstagramVM != null && this.InstagramVM.ProcessState == ProcessState.Paused
					  ? @"..\Images\yellow_light.png"
					  : @"..\Images\red_light.png";
			}
		}
		

		/// <summary>
		/// Gets a value indicating whether instagram process is running.
		/// </summary>
		/// <value>
		/// <c>true</c> if instagram process is running; otherwise, <c>false</c>.
		/// </value>
		public bool InstagramProcessRunning
		{
			get { return this.InstagramVM != null && this.InstagramVM.ProcessRunning; }
		}
		
		/// <summary>
		/// Property change notify handler for sub view models.
		/// </summary>
		public void InstagramNotifyHandler()
		{
			this.RaisePropertyChanged("InstagramProgressBulb");
			this.RaisePropertyChanged("InstagramProcessRunning");
		}

		#endregion

		#region twitter

		/// <summary>
		/// Gets the twitter progress bulb.
		/// </summary>
		/// <value>
		/// The twitter progress bulb.
		/// </value>
		public string TwitterProgressBulb
		{
			get { return @"..\Images\red_light.png"; }
		}

		#endregion

		#region facebook

		/// <summary>
		/// Gets the facebook progress bulb.
		/// </summary>
		/// <value>
		/// The facebook progress bulb.
		/// </value>
		public string FacebookProgressBulb
		{
			get { return @"..\Images\red_light.png"; }
		}

		#endregion

		#region tumblr

		/// <summary>
		/// Gets the tumblr progress bulb.
		/// </summary>
		/// <value>
		/// The tumblr progress bulb.
		/// </value>
		public string TumblrProgressBulb
		{
			get { return @"..\Images\red_light.png"; }
		}

		#endregion
	}
}