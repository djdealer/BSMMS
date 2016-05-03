using InstaFollow.Core.Context;
using InstaFollow.Core.UI.View;
using InstaFollow.Core.UI.ViewModel;

namespace InstaFollow.Core.UI.Command
{
	public class InfoCommand : BaseContextCommand<IInfoContext>
	{
		/// <summary>
		/// Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		public override void Execute(object parameter)
		{
			this.CurrentContext.WindowService.CreateAndShowWindowModal<IInfoView, InfoViewModel>();
		}

		/// <summary>
		/// Evaluates if the command can be executed.
		/// </summary>
		/// <returns>
		/// True if command can be executed, otherwise false.
		/// </returns>
		protected internal override bool EvaluateCanExecute()
		{
			return true;
		}
	}
}