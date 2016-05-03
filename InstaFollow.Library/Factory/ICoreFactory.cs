using System.Windows.Input;
using InstaFollow.Core.Container;
using InstaFollow.Core.Context;
using InstaFollow.Core.UI;
using InstaFollow.Core.UI.Command;
using InstaFollow.Core.UI.View;
using InstaFollow.Core.UI.ViewModel;

namespace InstaFollow.Core.Factory
{
	public interface ICoreFactory
	{
		T CreateContextCommand<T, TContext>(TContext context) where T : IContextCommand<TContext> where TContext : class, ICommandContext;
		T CreateCommand<T>() where T : ICommand;
		T CreateViewModel<T>(IWindowService windowService, ICoreFactory coreFactory) where T : IBaseViewModel;
		T CreateWindow<T>(IBaseViewModel viewModel) where T : IBaseWindow;
	}
}