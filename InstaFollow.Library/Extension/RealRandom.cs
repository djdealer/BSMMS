using System;

namespace InstaFollow.Library.Extension
{
	public class RealRandom : IRandomizer
	{
		public int Generate(int max)
		{
			return new Random().Next(0, max);
		}

		public int Generate(int min, int max)
		{
			return new Random().Next(min, max);
		}
	}
}