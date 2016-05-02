using InstaFollow.Core.Context;

namespace InstaFollow.Core.UI.Command
{
	public class VerifyLicenseCommand : BaseContextCommand<IVerifyContext>
	{
		private VerifyLicenseCommand() { }
		public override void Execute(object obj)
		{
			// TODO verify license

			this.CurrentContext.LicenseVerified = true;
			this.CurrentContext.CloseAction();
		}

		protected internal override bool EvaluateCanExecute()
		{
			return !string.IsNullOrEmpty(this.CurrentContext.LicenseKey) && !string.IsNullOrEmpty(this.CurrentContext.MachineKey);
		}
	}
}