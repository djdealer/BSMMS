using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstaFollow.Core.Enum;
using InstaFollow.Core.Extension;
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

		//[TestMethod]
		//public void SpinTest()
		//{
		//	var text0 = new TextSpinner().Spin("{The {quick|fast|speedy} brown fox {jumped|leaped|hopped} {over|right over|over the top of} the {lazy|sluggish|care-free|relaxing} dog.|{While|Although} just {taking|having} a{| little| quick} {siesta|nap} the dog was {startled|shocked|surprised} by a {quick|fast|speedy} {brown|dark brown|brownish} fox that {leaped|jumped} right {over|over the top of} him.}");
		//	var text1 = new TextSpinner().Spin("{The {quick|fast|speedy} brown fox {jumped|leaped|hopped} {over|right over|over the top of} the {lazy|sluggish|care-free|relaxing} dog.|{While|Although} just {taking|having} a{| little| quick} {siesta|nap} the dog was {startled|shocked|surprised} by a {quick|fast|speedy} {brown|dark brown|brownish} fox that {leaped|jumped} right {over|over the top of} him.}");
		//	var text2 = new TextSpinner().Spin("{The {quick|fast|speedy} brown fox {jumped|leaped|hopped} {over|right over|over the top of} the {lazy|sluggish|care-free|relaxing} dog.|{While|Although} just {taking|having} a{| little| quick} {siesta|nap} the dog was {startled|shocked|surprised} by a {quick|fast|speedy} {brown|dark brown|brownish} fox that {leaped|jumped} right {over|over the top of} him.}");
		//	var text3 = new TextSpinner().Spin("{The {quick|fast|speedy} brown fox {jumped|leaped|hopped} {over|right over|over the top of} the {lazy|sluggish|care-free|relaxing} dog.|{While|Although} just {taking|having} a{| little| quick} {siesta|nap} the dog was {startled|shocked|surprised} by a {quick|fast|speedy} {brown|dark brown|brownish} fox that {leaped|jumped} right {over|over the top of} him.}");
		//	var text4 = new TextSpinner().Spin("{The {quick|fast|speedy} brown fox {jumped|leaped|hopped} {over|right over|over the top of} the {lazy|sluggish|care-free|relaxing} dog.|{While|Although} just {taking|having} a{| little| quick} {siesta|nap} the dog was {startled|shocked|surprised} by a {quick|fast|speedy} {brown|dark brown|brownish} fox that {leaped|jumped} right {over|over the top of} him.}");
		//	var perm = new TextSpinner().Permutations("{The {quick|fast|speedy} brown fox {jumped|leaped|hopped} {over|right over|over the top of} the {lazy|sluggish|care-free|relaxing} dog.|{While|Although} just {taking|having} a{| little| quick} {siesta|nap} the dog was {startled|shocked|surprised} by a {quick|fast|speedy} {brown|dark brown|brownish} fox that {leaped|jumped} right {over|over the top of} him.}");
		//}
	}
}
