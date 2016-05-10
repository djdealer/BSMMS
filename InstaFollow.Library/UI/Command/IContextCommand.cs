using System.Windows.Input;
using BSMMS.Core.Context;

namespace BSMMS.Core.UI.Command
{
	public interface IContextCommand<in TContext> :
		ICommand
		where TContext : ICommandContext
	{
		void SetContext(TContext context);
	}
}