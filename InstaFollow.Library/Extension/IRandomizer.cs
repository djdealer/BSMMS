namespace InstaFollow.Core.Extension
{
	public interface IRandomizer
	{
		int Generate(int max);
		int Generate(int min, int max);
	}
}