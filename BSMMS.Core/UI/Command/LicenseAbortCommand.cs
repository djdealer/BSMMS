using BSMMS.Core.Context;

namespace BSMMS.Core.UI.Command
{
	public class LicenseAbortCommand : BaseContextCommand<IVerifyContext>
	{
		public override void Execute(object parameter)
		{
			this.CurrentContext.LicenseVerified = false;
			this.CurrentContext.CloseAction();
		}

		protected internal override bool EvaluateCanExecute()
		{
			return true;
		}
	}
}