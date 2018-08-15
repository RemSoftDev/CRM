using CRM.DAL.Entities;
using CRM.DAL.Repository;
using CRM.Log;
using CRM.Models;
using CRM.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using WebGrease.Css.Extensions;

namespace CRM.Managers
{
	public class EmailIgnoreNotifierManger : IEmailIgnoreNotifierManger
	{
		private static object syncRoot = new Object();
		private readonly IEmailService _emailService;
		private readonly long interval = 60000;
		private Timer _timer;
		private IgnoreNotifierWorkDayConfig _config;
		private IgnoreNotifierWorkDayConfig _nextDayConfig;

		private TimeSpan _timeForFirstNotification;
		private TimeSpan _timeForSacondNotification;
		private DayOfWeek _dayOfWeek;

		public bool IsFirstNotifyComplited { get; set; }

		public bool IsTimeChanged { get; set; }


		public EmailIgnoreNotifierManger(IUnitOfWork unitOfWork, IEmailService emailService)
		{
			_emailService = emailService;
			_dayOfWeek = DateTime.Now.DayOfWeek;
			var currentTime = DateTime.Now.TimeOfDay;

			_config = unitOfWork.IgnoreNotifierWorkDayConfigRepository
								 .FindBy(i => i.DayOfWeek == _dayOfWeek);

			_nextDayConfig = unitOfWork.IgnoreNotifierWorkDayConfigRepository
				.FindBy(i => i.DayOfWeek == _dayOfWeek + 1);

			_timeForFirstNotification = GetTimeForFirstNotification(currentTime);
			_timeForSacondNotification = GetTimeForSecondNotification(currentTime);
		}

		public void Start()
		{
			_timer = new Timer(CheckTime, null, 0, interval);
		}
		public void Stop()
		{
			_timer?.Dispose();
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

			if (!IsTimeChanged && IsWorkDayEnd(time))
			{
				ChangeTimeForNotification();
			}

			if (!IsFirstNotifyComplited && time >= _timeForFirstNotification && _dayOfWeek == DateTime.Now.DayOfWeek)
			{
				Logger.InfoLogContext.Debug("First notify");
				FirstNotify();
			}

			if (IsFirstNotifyComplited && time >= _timeForSacondNotification && _dayOfWeek == DateTime.Now.DayOfWeek)
			{
				Logger.InfoLogContext.Debug("Second notify");
				SecondNotify();
			}
		}


		private void ChangeTimeForNotification()
		{
			_timeForFirstNotification = _nextDayConfig.StartWorkTime + _config.IgnoreNotifierConfig.FirstDuration;
			_timeForSacondNotification = _nextDayConfig.StartWorkTime + _config.IgnoreNotifierConfig.SecondDuration;
			_dayOfWeek++;
			IsTimeChanged = true;

			Logger.InfoLogContext.Debug("Work day will ended soon, change ");
		}

		private bool IsWorkDayEnd(TimeSpan currentTime)
		{
			var timeToNotify = currentTime + _config.IgnoreNotifierConfig.FirstDuration;

			return _config.EndWorkTime < timeToNotify;
		}

		private void FirstNotify()
		{
			GenerateAndSendEmails(_config.IgnoreNotifierConfig.FirstRecipients);
			IsFirstNotifyComplited = true;
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
					Subject = _config.IgnoreNotifierConfig.EmailSubject, //TODO 
					Body = _config.IgnoreNotifierConfig.EmailBody
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