using BSMMS.Core.Factory;

namespace BSMMS.Core.Context
{
	public interface IInfoContext : ICommandContext
	{
		IWindowService WindowService { get; }
	}
}