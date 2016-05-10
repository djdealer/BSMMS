using BSMMS.Core.Context;
using BSMMS.Core.Enum;
using BSMMS.Core.Factory;
using BSMMS.Core.UI.Command;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BSMMS.Test.Command
{
	[TestClass]
	public class StopProcessCommandFixture
	{
		[TestMethod]
		public void ExecuteSetsStopFlag()
		{
			var contextMoq = new Mock<IProcessStateContext>();
			contextMoq.SetupAllProperties();

			var unit = CoreFactory.Instance.CreateContextCommand<StopProcessCommand, IProcessStateContext>(contextMoq.Object);
			unit.Execute(null);

			Assert.AreEqual(contextMoq.Object.ProcessState, ProcessState.Stopped);
		}

		[TestMethod]
		public void CanExecuteReturnsCorrectValue()
		{
			var contextMoq = new Mock<IProcessStateContext>();
			contextMoq.SetupAllProperties();

			var unit = CoreFactory.Instance.CreateContextCommand<StopProcessCommand, IProcessStateContext>(contextMoq.Object);

			Assert.AreEqual(unit.CanExecute(null), false);

			contextMoq.Setup(x => x.ProcessState).Returns(ProcessState.Running);
			unit = CoreFactory.Instance.CreateContextCommand<StopProcessCommand, IProcessStateContext>(contextMoq.Object);

			Assert.AreEqual(unit.CanExecute(null), true);

			contextMoq.Setup(x => x.ProcessState).Returns(ProcessState.Stopped);
			unit = CoreFactory.Instance.CreateContextCommand<StopProcessCommand, IProcessStateContext>(contextMoq.Object);

			Assert.AreEqual(unit.CanExecute(null), false);
		}
	}
}
