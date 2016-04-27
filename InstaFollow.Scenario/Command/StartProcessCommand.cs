using System;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Threading;
using InstaFollow.Library.Container;
using InstaFollow.Library.Enum;
using InstaFollow.Library.Exceptions;
using InstaFollow.Library.Extension;
using InstaFollow.Scenario.Strategy;
using InstaFollow.Scenario.Context;
using log4net;

namespace InstaFollow.Scenario.Command
{
	public class StartProcessCommand : BaseContextCommand<IStartProcessContext>, IExploreContext
	{
		private readonly ILog log = LogManager.GetLogger(typeof(StartProcessCommand));

		/// <summary>
		/// Prevents a default instance of the <see cref="StartProcessCommand"/> class from being created.
		/// </summary>
		private StartProcessCommand() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="StartProcessCommand"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		public StartProcessCommand(IStartProcessContext context) : base(context) { }

		#region context implementations

		public event PropertyChangedEventHandler PropertyChanged;
		public string UserName { get { return this.CurrentContext.UserName; } }
		public string Keywords { get { return this.CurrentContext.Keywords; } }
		public string CommentString { get { return this.CurrentContext.CommentString; } }
		public bool Like { get { return this.CurrentContext.Like; } }
		public bool Follow { get { return this.CurrentContext.Follow; } }
		public bool Comment { get { return this.CurrentContext.Comment; } set { this.CurrentContext.Comment = value; }}
		public bool Paging { get { return this.CurrentContext.Paging; } }
		public TimeoutRangeContainer TimeoutRange { get { return this.CurrentContext.TimeoutRange; } }

		public void UpdateCurrentImage(string imageUrl)
		{
			ThreadDispatcher.Invoke(() => this.CurrentContext.UpdateCurrentImage(imageUrl));
		}

		public ProcessState ProcessState
		{
			get { return this.CurrentContext.ProcessState; }
			set { this.CurrentContext.ProcessState = value; }
		}

		#endregion

		/// <summary>
		/// Executes the command scenario.
		/// Called by UI.
		/// </summary>
		/// <param name="obj">An object. (currently not used, so it is null).</param>
		/// <exception cref="InstagramException">An error occured during login!</exception>
		public override void Execute(object obj)
		{
			try
			{
				if (!InstagramInstagramHttpContainer.Instance.InstagramLogin(this.CurrentContext.UserName, this.CurrentContext.Password))
				{
					throw new InstagramException("An error occured during login!");
				}

				var strategy = new ExploreStrategy(this);

				new Thread(() => strategy.Explore()).Start();
			}
			catch (InstagramException iex)
			{
				this.HandleException(iex);
			}
			catch (Exception ex)
			{
				this.HandleException(ex);
			}
		}

		public void HandleException(Exception ex)
		{
			// TODO message box
			this.ProcessState = ProcessState.Error;

			if (ex is InstagramException)
			{
				log.Error(ex.Message);
			}
			else
			{
				log.Fatal(ex.Message);
			}
		}

		/// <summary>
		/// Evaluates if this command can be executed or not.
		/// </summary>
		/// <returns>True if execution is allowed, false otherwise.</returns>
		protected internal override bool EvaluateCanExecute()
		{
			return !string.IsNullOrEmpty(this.CurrentContext.UserName) &&
				 !string.IsNullOrEmpty(this.CurrentContext.Password) &&
					  !string.IsNullOrEmpty(this.CurrentContext.Keywords) &&
					  this.CurrentContext.ProcessState != ProcessState.Running &&
						(this.CurrentContext.Like || this.CurrentContext.Follow || this.CurrentContext.Comment);
		}
	}
}
