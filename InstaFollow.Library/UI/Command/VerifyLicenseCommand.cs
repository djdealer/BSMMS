using System;
using InstaFollow.Core.Context;
using InstaFollow.Core.Extension;

namespace InstaFollow.Core.UI.Command
{
	public class VerifyLicenseCommand : BaseContextCommand<IVerifyContext>
	{
		private VerifyLicenseCommand() { }
		public override void Execute(object obj)
		{
			this.CurrentContext.LicenseVerified = LicenseService.Instance.IsLicenseCodeValid(this.CurrentContext.LicenseKey);

			if (!this.CurrentContext.LicenseVerified)
			{
				throw new Exception("Code not valid!"); // TODO own exception
			}

			this.CurrentContext.CloseAction();
		}

		protected internal override bool EvaluateCanExecute()
		{
			return !string.IsNullOrEmpty(this.CurrentContext.LicenseKey) && !string.IsNullOrEmpty(this.CurrentContext.MachineKey);
		}
	}
}