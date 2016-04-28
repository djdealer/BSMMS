using System;

namespace InstaFollow.Core.Exceptions
{
	public class InstagramException : Exception
	{
		public InstagramException(string message) : base(message)
		{	
		}
	}
}