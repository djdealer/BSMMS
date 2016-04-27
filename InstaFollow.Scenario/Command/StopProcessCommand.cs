using System.ComponentModel;
using InstaFollow.Library.Enum;
using InstaFollow.Scenario.Context;

namespace InstaFollow.Scenario.Command
{
	public class StopProcessCommand : BaseContextCommand<IProcessStateContext>
	{
		/// <summary>
		/// Prevents a default instance of the <see cref="StopProcessCommand"/> class from being created.
		/// </summary>
		private StopProcessCommand() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="StopProcessCommand"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		public StopProcessCommand(IProcessStateContext context) : base(context) { }

		/// <summary>
		/// Executes the command.
		/// Called by UI.
		/// </summary>
		/// <param name="obj">An object. (currently not used, so it is null).</param>
		public override void Execute(object obj)
		{
			this.CurrentContext.ProcessState = ProcessState.Stopped;
		}


		/// <summary>
		/// Evaluates if this command can be executed or not.
		/// </summary>
		/// <returns>True if execution is allowed, false otherwise.</returns>
		protected internal override bool EvaluateCanExecute()
		{
			return this.CurrentContext.ProcessState == ProcessState.Running;
		}
	}
}