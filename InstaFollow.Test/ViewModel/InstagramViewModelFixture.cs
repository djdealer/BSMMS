using System;
using InstaFollow.Core.Enum;
using InstaFollow.Core.Factory;
using InstaFollow.Core.UI.ViewModel;
using Moq;
using NUnit.Framework;

namespace InstaFollow.Test.ViewModel
{
	[TestFixture]
	public class InstagramViewModelFixture
	{
		[Test]
		public void DisableControlsTest()
		{
			var windowService = new Mock<IWindowService>();
			windowService.SetupAllProperties();

			var unit = CoreFactory.Instance.CreateViewModel<InstagramViewModel>(windowService.Object, CoreFactory.Instance);

			unit.ProcessState = ProcessState.Running;

			Assert.AreEqual(unit.ProcessRunning, true);
			Assert.AreEqual(unit.CommentsEnabled, false);
			Assert.AreEqual(unit.KeywordsEnabled, false);
			Assert.AreEqual(unit.UserNameEnabled, false);
			Assert.AreEqual(unit.PasswordEnabled, false);

			unit.ProcessState = ProcessState.Finished;

			Assert.AreEqual(unit.ProcessRunning, false);
			Assert.AreEqual(unit.CommentsEnabled, true);
			Assert.AreEqual(unit.KeywordsEnabled, true);
			Assert.AreEqual(unit.UserNameEnabled, true);
			Assert.AreEqual(unit.PasswordEnabled, true);

			Assert.AreEqual(unit.CommentBoxEnabled, false);

			unit.CommentString = "This is a nice test.";

			Assert.AreEqual(unit.CommentBoxEnabled, true);
		}

		[Test]
		[TestCase(10,100, false)]
		[TestCase(1, 50, false)]
		[TestCase(12, 1, true)]
		public void TimeoutTest(int min, int max, bool shouldThrow)
		{
			var windowService = new Mock<IWindowService>();
			windowService.SetupAllProperties();

			var unit = CoreFactory.Instance.CreateViewModel<InstagramViewModel>(windowService.Object, CoreFactory.Instance);

			unit.MinTimeout = min;
			unit.MaxTimeout = max;

			if (!shouldThrow)
			{
				Assert.AreEqual(unit.TimeoutRange.MinTimeout, min);
				Assert.AreEqual(unit.TimeoutRange.MaxTimeout, max);
			}
			else
			{
				Assert.Throws<Exception>(() =>
				{
					var maxTimeout = unit.TimeoutRange.MaxTimeout;
				});

				Assert.Throws<Exception>(() =>
				{
					var minTimeout = unit.TimeoutRange.MinTimeout;
				});
			}
		}
	}
}
