using System;

namespace BSMMS.Core.Context
{
	public interface IVerifyContext : ICommandContext
	{
		string MachineKey { get; set; }
		string LicenseKey { get; }
		bool LicenseVerified { get; set; }

		Action CloseAction { get; }
	}
}