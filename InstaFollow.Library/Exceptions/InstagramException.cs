using System;

namespace InstaFollow.Library.Exceptions
{
	public class InstagramException : Exception
	{
		public InstagramException(string message) : base(message)
		{	
		}
	}
}