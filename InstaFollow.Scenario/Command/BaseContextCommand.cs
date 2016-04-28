using System;
using System.ComponentModel;
using System.Windows.Input;
using InstaFollow.Core.Context;
using InstaFollow.Core.UI.Command;
using MvvmFoundation.Wpf;

namespace InstaFollow.Scenario.Command
{
	public abstract class BaseContextCommand<TContext> : 
		IContextCommand<TContext>
		where TContext : class, ICommandContext
	{
		private TContext currentContext;
		private bool canExecute;

		public event EventHandler CanExecuteChanged;

		/// <summary>
		/// Sets the context.
		/// </summary>
		/// <param name="context">The context.</param>
		public void SetContext(TContext context)
		{
			this.CurrentContext = context;
		}

		/// <summary>
		/// Defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
		/// <returns>
		/// true if this command can be executed; otherwise, false.
		/// </returns>
		public virtual bool CanExecute(object parameter)
		{
			return this.canExecute;
		}

		/// <summary>
		/// Executes the command.
		/// </summary>
		/// <param name="obj">The object.</param>
		public abstract void Execute(object obj);

		/// <summary>
		/// Gets or sets the current context.
		/// </summary>
		/// <value>
		/// The current context.
		/// </value>
		protected internal TContext CurrentContext
		{
			get
			{
				return this.currentContext;
			}
			set
			{
				if (this.currentContext == value)
					return;

				if (this.currentContext != null)
					this.currentContext.PropertyChanged -= this.ContextPropertyChanged;

				this.currentContext = value;

				if (this.currentContext != null)
					this.currentContext.PropertyChanged += this.ContextPropertyChanged;

				this.UpdateCanExecute();
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseContextCommand{TContext}"/> class.
		/// </summary>
		protected BaseContextCommand() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseContextCommand{TContext}"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		protected BaseContextCommand(TContext context)
		{
			this.CurrentContext = context;
		}

		/// <summary>
		/// Updates the value of can execute.
		/// </summary>
		internal void UpdateCanExecute()
		{
			this.canExecute = this.CurrentContext != null && this.EvaluateCanExecute();
			if (this.CanExecuteChanged != null)
			{
				this.CanExecuteChanged(null, null);
			}
		}

		protected internal abstract bool EvaluateCanExecute();

		protected virtual void Update()
		{
			this.UpdateCanExecute();
		}

		private void ContextPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			this.Update();
		}
	}
}