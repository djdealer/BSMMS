using BSMMS.Core.Context;

namespace BSMMS.Core.Strategy
{
	public abstract class BaseContextStrategy<T> where T : class, ICommandContext
	{
		protected internal T CurrentContext { get; set; }

		protected BaseContextStrategy(T context)
		{
			this.CurrentContext = context;
		}
	}
}