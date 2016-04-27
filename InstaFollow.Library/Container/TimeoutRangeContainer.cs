namespace InstaFollow.Library.Container
{
	public class TimeoutRangeContainer
	{
		private TimeoutRangeContainer() { }

		public TimeoutRangeContainer(int maxTimeout, int minTimeout)
		{
			this.MaxTimeout = maxTimeout;
			this.MinTimeout = minTimeout;
		}

		public int MinTimeout { get; set; }
		public int MaxTimeout { get; set; }
	}
}