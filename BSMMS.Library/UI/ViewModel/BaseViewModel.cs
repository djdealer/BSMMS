using System;
using BSMMS.Core.Factory;
using MvvmFoundation.Wpf;

namespace BSMMS.Core.UI.ViewModel
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