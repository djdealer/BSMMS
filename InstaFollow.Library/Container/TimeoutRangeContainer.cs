using System;

namespace InstaFollow.Core.Container
{
	public class TimeoutRangeContainer
	{
		private TimeoutRangeContainer() { }

		public TimeoutRangeContainer(int minTimeout, int maxTimeout)
		{
			if (maxTimeout < minTimeout)
			{
				throw new Exception("Maximum value should be higher than minimum value!");
			}

			this.MaxTimeout = maxTimeout;
			this.MinTimeout = minTimeout;
		}

		public int MinTimeout { get; set; }
		public int MaxTimeout { get; set; }
	}
}