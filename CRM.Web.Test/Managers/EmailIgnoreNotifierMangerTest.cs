using CRM.DAL.Repository;
using CRM.Managers;
using CRM.Web.Test.Stubs.Repositories;
using Moq;
using NUnit.Framework;
using System;


namespace CRM.Web.Test.Managers
{
	[TestFixture]
	public class EmailIgnoreNotifierMangerTest
	{
		[Test]
		public void GetTimeForSecondNotificationTest()
		{
			TimeSpan currentTime = new TimeSpan(12, 0, 0);

			var timeForNotify = Subject.GetTimeForSecondNotification(currentTime);

			Assert.AreEqual(timeForNotify, new TimeSpan(13, 0, 0));
		}

		[Test]
		public void GetTimeForFirstNotificationTest()
		{
			TimeSpan currentTime = new TimeSpan(12, 0, 0);

			var timeForNotify = Subject.GetTimeForFirstNotification(currentTime);

			Assert.AreEqual(timeForNotify, new TimeSpan(12, 15, 0));
		}

		[SetUp]
		public void Init()
		{
			UnitOfWork = new Mock<IUnitOfWork>();

			var repository = new FaceIgnoreNotifierWorkDayConfigRepository();

			UnitOfWork.SetupGet(u => u.IgnoreNotifierWorkDayConfigRepository).Returns(repository);

			Subject = new EmailIgnoreNotifierManger(UnitOfWork.Object);
		}

		public Mock<IUnitOfWork> UnitOfWork { get; set; }
		private EmailIgnoreNotifierManger Subject { get; set; }

	}
}
