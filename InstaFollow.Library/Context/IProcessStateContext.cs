using BSMMS.Core.Enum;

namespace BSMMS.Core.Context
{
	public interface IProcessStateContext : ICommandContext
	{
		ProcessState ProcessState { get; set; }
	}
}