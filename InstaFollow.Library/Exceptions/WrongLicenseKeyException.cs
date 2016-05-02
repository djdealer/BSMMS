using System;

namespace InstaFollow.Core.Exceptions
{
	public class WrongLicenseKeyException : Exception
	{
		public WrongLicenseKeyException(string message)
			: base(message)
		{
		}
	}
}