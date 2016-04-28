using System;
using System.Windows.Input;
using InstaFollow.Core.Container;
using InstaFollow.Core.Context;
using InstaFollow.Core.UI;
using InstaFollow.Core.UI.Command;
using InstaFollow.Core.UI.ViewModel;

namespace InstaFollow.Core.Factory
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
			var inst = (T)Activator.CreateInstance(typeof(T), true);

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
			return (T)Activator.CreateInstance(typeof(T), true);
		}

		/// <summary>
		/// Creates a view model.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns>A new instance of T.</returns>
		public T CreateViewModel<T>(IWindowService windowService, ICoreFactory coreFactory) where T : IBaseViewModel
		{
			var inst = (T) Activator.CreateInstance(typeof(T), true);
			
			inst.WindowService = windowService;
			inst.CoreFactory = coreFactory;

			return inst;
		}
	}
}