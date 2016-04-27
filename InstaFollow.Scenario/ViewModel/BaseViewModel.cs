using System;
using System.Windows.Input;
using InstaFollow.Scenario.Command;
using InstaFollow.Scenario.Context;
using MvvmFoundation.Wpf;

namespace InstaFollow.Scenario.ViewModel
{
	public abstract class BaseViewModel : ObservableObject
	{
		/// <summary>
		/// Creates a context command.
		/// </summary>
		/// <typeparam name="T">The type of the command. Should implement ICommand.</typeparam>
		/// <typeparam name="TContext">The type of the context.</typeparam>
		/// <returns>A new instance.</returns>
		public T CreateContextCommand<T, TContext>()
			where T : BaseContextCommand<TContext>
			where TContext : class, ICommandContext
		{
			var instance = (T)Activator.CreateInstance(typeof(T), true);

			instance.CurrentContext = this as TContext;

			return instance;
		}

		/// <summary>
		/// Creates a simple command.
		/// </summary>
		/// <typeparam name="T">The type of the command. Should implement ICommand.</typeparam>
		/// <returns>A new instance of T.</returns>
		public T CreateCommand<T>() where T : ICommand
		{
			var instance = (T)Activator.CreateInstance(typeof(T), true);

			return instance;
		}
	}
}