using System;
using BSMMS.Core.Context;
using BSMMS.Core.Exceptions;
using BSMMS.Core.Extension;

namespace BSMMS.Core.UI.Command
{
	public class VerifyLicenseCommand : BaseContextCommand<IVerifyContext>
	{
		private VerifyLicenseCommand() { }
		public override void Execute(object obj)
		{
			this.CurrentContext.LicenseVerified = LicenseService.Instance.IsLicenseCodeValid(this.CurrentContext.LicenseKey);

			if (!this.CurrentContext.LicenseVerified)
			{
				throw new WrongLicenseKeyException("Code not valid!");
			}

			LicenseService.Instance.WriteLicenseCodeToRegistry(this.CurrentContext.LicenseKey);

			this.CurrentContext.CloseAction();
		}

		protected internal override bool EvaluateCanExecute()
		{
			return !string.IsNullOrEmpty(this.CurrentContext.LicenseKey) && !string.IsNullOrEmpty(this.CurrentContext.MachineKey);
		}
	}
}