using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstaFollow.Library.Enum;
using InstaFollow.Scenario.Command;
using InstaFollow.Scenario.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace InstaFollow.Test.Command
{
	[TestClass]
	public class StopProcessCommandFixture
	{
		[TestMethod]
		public void ExecuteSetsStopFlag()
		{
			var contextMoq = new Mock<IProcessStateContext>();
			contextMoq.SetupAllProperties();

			var unit = new StopProcessCommand(contextMoq.Object);
			unit.Execute(null);

			Assert.AreEqual(contextMoq.Object.ProcessState, ProcessState.Stopped);
		}

		[TestMethod]
		public void CanExecuteReturnsCorrectValue()
		{
			var contextMoq = new Mock<IProcessStateContext>();
			contextMoq.SetupAllProperties();

			var unit = new StopProcessCommand(contextMoq.Object);

			Assert.AreEqual(unit.CanExecute(null), false);

			contextMoq.Setup(x => x.ProcessState).Returns(ProcessState.Running);
			unit = new StopProcessCommand(contextMoq.Object);

			Assert.AreEqual(unit.CanExecute(null), true);

			contextMoq.Setup(x => x.ProcessState).Returns(ProcessState.Stopped);
			unit = new StopProcessCommand(contextMoq.Object);

			Assert.AreEqual(unit.CanExecute(null), false);
		}
	}
}
