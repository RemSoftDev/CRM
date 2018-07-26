using CRM.DAL.Entities;
using CRM.DAL.Repository;
using CRM.Models;
using CRM.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using CRM.Log;
using WebGrease.Css.Extensions;

namespace CRM.Managers
{
	public class EmailIgnoreNotifierManger : IEmailIgnoreNotifierManger
	{
		private readonly IEmailService _emailService;
		private readonly long interval = 60000;
		private Timer _timer;
		private IgnoreNotifierWorkDayConfig _config;

		private readonly TimeSpan _timeForFirstNotification;
		private readonly TimeSpan _timeForSacondNotification;

		private bool _firstNotifyComplited;

		public EmailIgnoreNotifierManger(IUnitOfWork unitOfWork, IEmailService emailService)
		{
			_emailService = emailService;
			var dayOfWeek = DateTime.Now.DayOfWeek;
			var currentTime = DateTime.Now.TimeOfDay;

			_config = unitOfWork.IgnoreNotifierWorkDayConfigRepository
								 .FindBy(i => i.DayOfWeek == dayOfWeek);

			_timeForFirstNotification = GetTimeForFirstNotification(currentTime);
			_timeForSacondNotification = GetTimeForSecondNotification(currentTime);
		}

		public void Start()
		{
			_timer = new Timer(CheckTime, null, 0, interval);
		}

		internal TimeSpan GetTimeForFirstNotification(TimeSpan currentTime)
		{
			return currentTime.Add(_config.IgnoreNotifierConfig.FirstDuration);
		}

		internal TimeSpan GetTimeForSecondNotification(TimeSpan currentTime)
		{
			return currentTime.Add(_config.IgnoreNotifierConfig.SecondDuration);
		}

		private void CheckTime(object state)
		{
			var time = DateTime.Now.TimeOfDay;

			//if (IsWorkDayEnd(time))
			//{

			//}

			if (!_firstNotifyComplited && time >= _timeForFirstNotification)
			{
				Logger.InfoLogContext.Debug("First notify");
				FirstNotify();
			}

			if (_firstNotifyComplited && time >= _timeForSacondNotification)
			{
				Logger.InfoLogContext.Debug("Second notify");
				SecondNotify();
			}
		}

		private bool IsWorkDayEnd(TimeSpan currentTime)
		{
			var timeToNotify = currentTime + _timeForFirstNotification;

			return _config.EndWorkTime < timeToNotify;
		}

		private void FirstNotify()
		{
			GenerateAndSendEmails(_config.IgnoreNotifierConfig.FirstRecipients);
			_firstNotifyComplited = true;
		}


		private void SecondNotify()
		{
			GenerateAndSendEmails(_config.IgnoreNotifierConfig.SecondRecipients);
			_timer.Dispose();
		}

		internal void GenerateAndSendEmails(IEnumerable<User> recipients)
		{
			var emails = CreateEmailTo(recipients);
			SendEmailTo(emails);
			Logger.InfoLogContext.Debug("Generate and send emails");
		}

		private IEnumerable<EmailViewModel> CreateEmailTo(IEnumerable<User> recipients)
		{
			var emails = new List<EmailViewModel>();

			foreach (var recipient in recipients)
			{
				var email = new EmailViewModel
				{
					To = recipient.Email,
					Subject = "Notification", //TODO 
					Body = "You ignore"
				};

				emails.Add(email);
			}

			return emails;
		}

		private void SendEmailTo(IEnumerable<EmailViewModel> emails)
		{
			emails.ForEach(e => _emailService.SendEmail(e));
		}

	}
}