using InstaFollow.Core.Factory;

namespace InstaFollow.Core.Context
{
	public interface IInfoContext : ICommandContext
	{
		IWindowService WindowService { get; }
	}
}