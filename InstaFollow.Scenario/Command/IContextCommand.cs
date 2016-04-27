using System.Windows.Input;
using InstaFollow.Scenario.Context;

namespace InstaFollow.Scenario.Command
{
	public interface IContextCommand<out TContext> :
		ICommand
		where TContext : ICommandContext
	{

	}
}