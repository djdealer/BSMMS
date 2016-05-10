using System;

namespace BSMMS.Core.Extension
{
	public class Randomizer : Random, IRandomizer
	{
		private static IRandomizer instance;

		private Randomizer() { }

		public static IRandomizer Instance
		{
			get { return instance ?? (instance = new Randomizer()); }
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