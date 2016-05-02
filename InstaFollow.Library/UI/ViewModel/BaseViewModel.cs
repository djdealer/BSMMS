using System;
using InstaFollow.Core.Factory;
using MvvmFoundation.Wpf;

namespace InstaFollow.Core.UI.ViewModel
{
	public abstract class BaseViewModel : ObservableObject, IBaseViewModel
	{
		public IWindowService WindowService { get; set; }
		public ICoreFactory CoreFactory { get; set; }
		public Action CloseAction { get; set; }

		protected BaseViewModel() { }

		public virtual void Init()
		{
			
		}
	}
}