namespace InstaFollow.Core.Container
{
	public class TimeoutRangeContainer
	{
		private TimeoutRangeContainer() { }

		public TimeoutRangeContainer(int minTimeout, int maxTimeout)
		{
			this.MaxTimeout = maxTimeout;
			this.MinTimeout = minTimeout;
		}

		public int MinTimeout { get; set; }
		public int MaxTimeout { get; set; }
	}
}