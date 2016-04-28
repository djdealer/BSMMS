using InstaFollow.Core.Enum;

namespace InstaFollow.Core.Context
{
	public interface IProcessStateContext : ICommandContext
	{
		ProcessState ProcessState { get; set; }
	}
}