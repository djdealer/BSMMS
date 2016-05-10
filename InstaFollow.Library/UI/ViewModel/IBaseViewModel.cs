using BSMMS.Core.Factory;

namespace BSMMS.Core.UI.ViewModel
{
	public interface IBaseViewModel
	{
		IWindowService WindowService { get; set; }
		ICoreFactory CoreFactory { get; set; }
	}
}