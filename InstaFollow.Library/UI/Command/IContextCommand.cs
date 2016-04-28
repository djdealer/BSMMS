using System.Windows.Input;
using InstaFollow.Core.Context;

namespace InstaFollow.Core.UI.Command
{
	public interface IContextCommand<in TContext> :
		ICommand
		where TContext : ICommandContext
	{
		void SetContext(TContext context);
	}
}