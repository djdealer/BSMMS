using System;

namespace BSMMS.Core.Exceptions
{
	public class WrongLicenseKeyException : Exception
	{
		public WrongLicenseKeyException(string message)
			: base(message)
		{
		}
	}
}