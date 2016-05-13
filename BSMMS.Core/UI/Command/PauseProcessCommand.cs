using BSMMS.Core.Context;
using BSMMS.Core.Enum;

namespace BSMMS.Core.UI.Command
{
	public class PauseProcessCommand : BaseContextCommand<IProcessStateContext>
	{
		public override void Execute(object obj)
		{
			this.CurrentContext.ProcessState = ProcessState.Paused;
		}

		protected internal override bool EvaluateCanExecute()
		{
			return this.CurrentContext.ProcessState == ProcessState.Running;
		}
	}
}