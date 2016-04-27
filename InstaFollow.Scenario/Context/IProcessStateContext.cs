using InstaFollow.Library.Enum;

namespace InstaFollow.Scenario.Context
{
	public interface IProcessStateContext : ICommandContext
	{
		ProcessState ProcessState { get; set; }
	}
}