using System.Windows.Input;
using InstaFollow.Library.Container;
using InstaFollow.Library.Enum;
using InstaFollow.Library.Extension;
using InstaFollow.Scenario.Command;
using InstaFollow.Scenario.Context;

namespace InstaFollow.Scenario.ViewModel
{
	public class MainViewModel : BaseViewModel, IStartProcessContext
	{
		private string userName, password, keywords, commentString, currentImage;
		private ProcessState processState;
		private bool like, follow, comment;
		private bool paging;

		/// <summary>
		/// Initializes a new instance of the <see cref="MainViewModel"/> class.
		/// </summary>
		public MainViewModel()
		{
			ThreadDispatcher.Initialize();
			this.StartProcessCommand = this.CreateContextCommand<StartProcessCommand, IStartProcessContext>();
			this.StopProcessCommand = this.CreateContextCommand<StopProcessCommand, IProcessStateContext>();
		}

		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>
		/// The name of the user.
		/// </value>
		public string UserName
		{
			get { return userName; }
			set
			{
				userName = value;
				this.RaisePropertyChanged("UserName");
				this.RaisePropertyChanged("StartCommandEnabled");
				this.RaisePropertyChanged("StopCommandEndabled");
				this.RaisePropertyChanged("StartButtonImage");
				this.RaisePropertyChanged("StopButtonImage");
			}
		}

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>
		/// The password.
		/// </value>
		public string Password
		{
			get { return password; }
			set
			{
				password = value;
				this.RaisePropertyChanged("Password");
				this.RaisePropertyChanged("StartCommandEnabled");
				this.RaisePropertyChanged("StopCommandEndabled");
				this.RaisePropertyChanged("StartButtonImage");
				this.RaisePropertyChanged("StopButtonImage");
			}
		}

		/// <summary>
		/// Gets or sets the keywords.
		/// </summary>
		/// <value>
		/// The keywords.
		/// </value>
		public string Keywords
		{
			get { return keywords; }
			set
			{
				keywords = value;
				this.RaisePropertyChanged("Keywords");
				this.RaisePropertyChanged("StartCommandEnabled");
				this.RaisePropertyChanged("StopCommandEndabled");
				this.RaisePropertyChanged("StartButtonImage");
				this.RaisePropertyChanged("StopButtonImage");
			}
		}

		/// <summary>
		/// Gets or sets the CommentString.
		/// </summary>
		/// <value>
		/// The CommentString.
		/// </value>
		public string CommentString
		{
			get { return commentString; }
			set
			{
				commentString = value;
				this.RaisePropertyChanged("CommentString");
				this.RaisePropertyChanged("CommentBoxEnabled");
				this.RaisePropertyChanged("StartCommandEnabled");
				this.RaisePropertyChanged("StopCommandEndabled");
				this.RaisePropertyChanged("StartButtonImage");
				this.RaisePropertyChanged("StopButtonImage");
			}
		}

		/// <summary>
		/// Gets or sets the current image.
		/// </summary>
		/// <value>
		/// The current image.
		/// </value>
		public string CurrentImage
		{
			get { return currentImage; }
			set
			{
				currentImage = value;
				this.RaisePropertyChanged("CurrentImage");
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the strategy should like or not.
		/// </summary>
		/// <value>
		///   <c>true</c> if enabled; otherwise, <c>false</c>.
		/// </value>
		public bool Like
		{
			get { return this.like; }
			set
			{
				this.like = value;
				this.RaisePropertyChanged("Like");
				this.RaisePropertyChanged("StartCommandEnabled");
				this.RaisePropertyChanged("StopCommandEndabled");
				this.RaisePropertyChanged("StartButtonImage");
				this.RaisePropertyChanged("StopButtonImage");
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the strategy should follow or not.
		/// </summary>
		/// <value>
		///   <c>true</c> if enabled; otherwise, <c>false</c>.
		/// </value>
		public bool Follow
		{
			get { return follow; }
			set
			{
				this.follow = value;
				this.RaisePropertyChanged("Follow");
				this.RaisePropertyChanged("StartCommandEnabled");
				this.RaisePropertyChanged("StopCommandEndabled");
				this.RaisePropertyChanged("StartButtonImage");
				this.RaisePropertyChanged("StopButtonImage");
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the strategy should comment or not.
		/// </summary>
		/// <value>
		///   <c>true</c> if enabled; otherwise, <c>false</c>.
		/// </value>
		public bool Comment
		{
			get { return comment; }
			set
			{
				this.comment = value;
				this.RaisePropertyChanged("Comment");
				this.RaisePropertyChanged("StartCommandEnabled");
				this.RaisePropertyChanged("StopCommandEndabled");
				this.RaisePropertyChanged("StartButtonImage");
				this.RaisePropertyChanged("StopButtonImage");
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the stategy should use paging.
		/// </summary>
		/// <value>
		///   <c>true</c> if paging; otherwise, <c>false</c>.
		/// </value>
		public bool Paging
		{
			get { return this.paging; }
			set
			{
				this.paging = value;
				this.RaisePropertyChanged("Paging");
				this.RaisePropertyChanged("StartCommandEnabled");
				this.RaisePropertyChanged("StopCommandEndabled");
				this.RaisePropertyChanged("StartButtonImage");
				this.RaisePropertyChanged("StopButtonImage");
			}
		}

		/// <summary>
		/// Gets a value indicating whether the comment tick box is enabled or not.
		/// </summary>
		/// <value>
		///   <c>true</c> if comment box enabled; otherwise, <c>false</c>.
		/// </value>
		public bool CommentBoxEnabled { get { return !string.IsNullOrEmpty(this.CommentString); } }

		/// <summary>
		/// Gets the timeout range.
		/// </summary>
		/// <value>
		/// The timeout range.
		/// </value>
		public TimeoutRangeContainer TimeoutRange
		{
			get { return new TimeoutRangeContainer(15, 6); } // TODO
		}

		/// <summary>
		/// Gets or sets the start process command.
		/// </summary>
		/// <value>
		/// The start process command.
		/// </value>
		public ICommand StartProcessCommand { get; set; }

		/// <summary>
		/// Gets a value indicating whether the start command can execute.
		/// </summary>
		/// <value>
		///   <c>true</c> if this command can start; otherwise, <c>false</c>.
		/// </value>
		public bool StartCommandEnabled { get { return this.StartProcessCommand.CanExecute(null); } }

		/// <summary>
		/// Gets or sets the stop process command.
		/// </summary>
		/// <value>
		/// The stop process command.
		/// </value>
		public ICommand StopProcessCommand { get; set; }

		/// <summary>
		/// Gets a value indicating whether the stop command can execute.
		/// </summary>
		/// <value>
		///   <c>true</c> if this command can start; otherwise, <c>false</c>.
		/// </value>
		public bool StopCommandEndabled { get { return this.StopProcessCommand.CanExecute(null); } }

		/// <summary>
		/// Gets the play button image.
		/// </summary>
		/// <value>
		/// The play button image.
		/// </value>
		public string StartButtonImage
		{
			get { return this.StartCommandEnabled ? @"..\Images\play.png" : @"..\Images\play_disabled.png"; }
		}

		/// <summary>
		/// Gets the stop button image.
		/// </summary>
		/// <value>
		/// The stop button image.
		/// </value>
		public string StopButtonImage
		{
			get { return this.StopCommandEndabled ? @"..\Images\stop.png" : @"..\Images\stop_disabled.png"; }
		}

		/// <summary>
		/// Gets or sets the state of the process.
		/// </summary>
		/// <value>
		/// The state of the process.
		/// </value>
		public ProcessState ProcessState
		{
			get { return this.processState; }
			set
			{
				this.processState = value;
				this.RaisePropertyChanged("ProcessState");
				this.RaisePropertyChanged("ProcessStateText");
				this.RaisePropertyChanged("DoMarquee");
				this.RaisePropertyChanged("StartCommandEnabled");
				this.RaisePropertyChanged("StopCommandEndabled");
				this.RaisePropertyChanged("StartButtonImage");
				this.RaisePropertyChanged("StopButtonImage");
			}
		}

		/// <summary>
		/// Gets the process state as text.
		/// </summary>
		/// <value>
		/// The process state as text.
		/// </value>
		public string ProcessStateText { get { return this.ProcessState + "."; } }

		/// <summary>
		/// Gets a value indicating whether the progress bar should marquee or not.
		/// </summary>
		/// <value>
		///   <c>true</c> if it should marquee; otherwise, <c>false</c>.
		/// </value>
		public bool DoMarquee { get { return this.ProcessState == ProcessState.Running; } }

		/// <summary>
		/// Updates the current image (thread safe).
		/// </summary>
		/// <param name="imageUrl">The image URL.</param>
		public void UpdateCurrentImage(string imageUrl)
		{
			this.CurrentImage = imageUrl;
		}
	}
}