using System;
using System.ComponentModel;
using System.Windows.Input;
using InstaFollow.Scenario.Context;
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

		public virtual bool CanExecute(object parameter)
		{
			return this.canExecute;
		}

		public abstract void Execute(object obj);

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

		protected BaseContextCommand() { }

		protected BaseContextCommand(TContext context)
		{
			this.CurrentContext = context;
		}

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