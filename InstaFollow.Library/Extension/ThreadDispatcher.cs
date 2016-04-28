using System;
using System.Threading;

namespace InstaFollow.Core.Extension
{
	public static class ThreadDispatcher
	{
		private static SynchronizationContext UiContext { get; set; }

		public static void Initialize()
		{
			if (UiContext == null)
			{
				UiContext = SynchronizationContext.Current;
			}
		}

		public static void InvokeAsync(Action action)
		{
			CheckInitialization();

			UiContext.Post(x => action(), null);
		}

		public static void Invoke(Action action)
		{
			CheckInitialization();

			if (UiContext == SynchronizationContext.Current)
			{
				action();
			}
			else
			{
				InvokeAsync(action);
			}
		}

		private static void CheckInitialization()
		{
			if (UiContext == null)
			{
				throw new InvalidOperationException("ThreadDispatcher is not initialized. Invoke Initialize() first.");
			}
		}
	}
}