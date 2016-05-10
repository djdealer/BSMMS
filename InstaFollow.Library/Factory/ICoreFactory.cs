using System.Windows.Input;
using BSMMS.Core.Context;
using BSMMS.Core.UI.View;
using BSMMS.Core.UI.Command;
using BSMMS.Core.UI.ViewModel;

namespace BSMMS.Core.Factory
{
	public interface ICoreFactory
	{
		T CreateContextCommand<T, TContext>(TContext context) where T : IContextCommand<TContext> where TContext : class, ICommandContext;
		T CreateCommand<T>() where T : ICommand;
		T CreateViewModel<T>(IWindowService windowService, ICoreFactory coreFactory) where T : IBaseViewModel;
		T CreateWindow<T>(IBaseViewModel viewModel) where T : IBaseWindow;
	}
}