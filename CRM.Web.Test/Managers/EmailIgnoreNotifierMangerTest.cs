using CRM.DAL.Entities;
using CRM.DAL.Repository;
using CRM.Managers;
using CRM.Models;
using CRM.Services;
using CRM.Web.Test.Stubs.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;


namespace CRM.Web.Test.Managers
{
	[TestFixture]
	public class EmailIgnoreNotifierMangerTest
	{
		private EmailIgnoreNotifierManger _subject;
		private Mock<IEmailService> _emailService;
		private Mock<IUnitOfWork> _unitOfWork;

		[Test]
		public void GenerateAndSendEmailsTest()
		{
			var users = new List<User>
			{
				new User
				{
					Email = "test1@hotmail.com"
				},
				new User
				{
					Email = "test2@hotmail.com"
				},
				new User
				{
					Email = "test2@hotmail.com"
				},
			};

			_subject.GenerateAndSendEmails(users);
			_emailService.Verify(e => e.SendEmail(It.IsAny<EmailViewModel>()), Times.Exactly(3));
		}


		[Test]
		public void GetTimeForSecondNotificationTest()
		{
			TimeSpan currentTime = new TimeSpan(12, 0, 0);

			var timeForNotify = _subject.GetTimeForSecondNotification(currentTime);

			Assert.AreEqual(timeForNotify, new TimeSpan(13, 0, 0));
		}

		[Test]
		public void GetTimeForFirstNotificationTest()
		{
			TimeSpan currentTime = new TimeSpan(12, 0, 0);

			var timeForNotify = _subject.GetTimeForFirstNotification(currentTime);

			Assert.AreEqual(timeForNotify, new TimeSpan(12, 15, 0));
		}

		[SetUp]
		public void Init()
		{
			_unitOfWork = new Mock<IUnitOfWork>();
			_emailService = new Mock<IEmailService>();

			var repository = new FaceIgnoreNotifierWorkDayConfigRepository();

			_unitOfWork.SetupGet(u => u.IgnoreNotifierWorkDayConfigRepository).Returns(repository);

			_subject = new EmailIgnoreNotifierManger(_unitOfWork.Object, _emailService.Object);
		}
	}
}
