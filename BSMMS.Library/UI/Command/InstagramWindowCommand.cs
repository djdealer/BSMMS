using BSMMS.Core.Context;
using BSMMS.Core.UI.View;
using BSMMS.Core.UI.ViewModel;

namespace BSMMS.Core.UI.Command
{
	public class InstagramWindowCommand : BaseContextCommand<IInstagramContext>
	{
		/// <summary>
		/// Executes the command.
		/// </summary>
		/// <param name="obj">The object.</param>
		public override void Execute(object obj)
		{
			var view = this.CurrentContext.WindowService.CreateAndShowWindow<IInstagramView, InstagramViewModel>(this.CurrentContext.InstagramVM);
			var instagramViewModel = view.ViewModel as InstagramViewModel;
			if (instagramViewModel != null)
			{
				instagramViewModel.NotifyMainVm = this.CurrentContext.InstagramNotifyHandler;
				this.CurrentContext.InstagramVM = instagramViewModel;
			}
		}

		/// <summary>
		/// Evaluates if the command can be executed.
		/// </summary>
		/// <returns>
		/// True if command can be executed, otherwise false.
		/// </returns>
		protected internal override bool EvaluateCanExecute()
		{
			return true;
		}
	}
}