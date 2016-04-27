using System;

namespace InstaFollow.Library.Extension
{
	public class RealRandom : Random, IRandomizer
	{
		private static IRandomizer instance;

		private RealRandom() { }

		public static IRandomizer Instance
		{
			get { return instance ?? (instance = new RealRandom()); }
		}

		public int Generate(int max)
		{
			return this.Next(max);
		}

		public int Generate(int min, int max)
		{
			return this.Next(min, max);
		}
	}
}