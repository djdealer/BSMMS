using System;
using System.Windows;
using System.Windows.Input;
using BSMMS.Core.Context;
using BSMMS.Core.UI.View;
using BSMMS.Core.UI.Command;
using BSMMS.Core.UI.ViewModel;

namespace BSMMS.Core.Factory
{
	public class CoreFactory : ICoreFactory
	{
		private static ICoreFactory instance;

		private CoreFactory() { }

		public static ICoreFactory Instance
		{
			get { return instance ?? (instance = new CoreFactory()); }
		}

		/// <summary>
		/// Creates a context command.
		/// </summary>
		/// <typeparam name="T">The type of the command. Should implement ICommand.</typeparam>
		/// <typeparam name="TContext">The type of the context.</typeparam>
		/// <returns>A new instance.</returns>
		public T CreateContextCommand<T, TContext>(TContext context)
			where T : IContextCommand<TContext>
			where TContext : class, ICommandContext
		{
			var inst = CustomActivator.CreateInstance<T>();

			inst.SetContext(context);

			return inst;
		}

		/// <summary>
		/// Creates a simple command.
		/// </summary>
		/// <typeparam name="T">The type of the command. Should implement ICommand.</typeparam>
		/// <returns>A new instance of T.</returns>
		public T CreateCommand<T>() where T : ICommand
		{
			return CustomActivator.CreateInstance<T>();
		}

		/// <summary>
		/// Creates a view model.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns>A new instance of T.</returns>
		public T CreateViewModel<T>(IWindowService windowService, ICoreFactory coreFactory) where T : IBaseViewModel
		{
			var inst = CustomActivator.CreateInstance<T>();
			
			inst.WindowService = windowService;
			inst.CoreFactory = coreFactory;

			return inst;
		}

		/// <summary>
		/// Creates the window.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="viewModel">The view model.</param>
		/// <returns>A new instance of T.</returns>
		public T CreateWindow<T>(IBaseViewModel viewModel) where T : IBaseWindow
		{
			var inst = CustomActivator.CreateInstance<T>();
			inst.AttachContext(viewModel);
			return inst;
		}
	}
}