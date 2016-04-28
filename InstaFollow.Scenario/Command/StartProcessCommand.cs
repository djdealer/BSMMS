using System;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Threading;
using InstaFollow.Core.Container;
using InstaFollow.Core.Context;
using InstaFollow.Core.Enum;
using InstaFollow.Core.Exceptions;
using InstaFollow.Core.Extension;
using InstaFollow.Scenario.Strategy;
using log4net;

namespace InstaFollow.Scenario.Command
{
	public class StartProcessCommand : BaseContextCommand<IExploreContext>
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
		public StartProcessCommand(IExploreContext context) : base(context) { }

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
				var strategy = new ExploreStrategy(this.CurrentContext);
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
			this.CurrentContext.ProcessState = ProcessState.Error;

			if (ex is InstagramException)
			{
				log.Error(ex.Message);
			}
			else
			{
				log.Fatal(ex.Message);
			}

			this.CurrentContext.HandleException(ex);
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
