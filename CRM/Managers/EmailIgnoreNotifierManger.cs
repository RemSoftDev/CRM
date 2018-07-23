using CRM.DAL.Entities;
using CRM.DAL.Repository;
using System;
using System.Threading;

namespace CRM.Managers
{
	public class EmailIgnoreNotifierManger
	{
		private readonly long interval = 60000;
		private Timer _timer;
		private IgnoreNotifierWorkDayConfig _config;

		private readonly TimeSpan _timeForFirstNotification;
		private readonly TimeSpan _timeForSacondNotification;

		private bool _firstNotifyComplited;

		public EmailIgnoreNotifierManger(IUnitOfWork unitOfWork)
		{
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

			if (!_firstNotifyComplited && time >= _timeForFirstNotification)
			{
				FirstNotify();
			}

			if (_firstNotifyComplited && time >= _timeForSacondNotification)
			{
				SecondNotify();
			}
		}

		private void FirstNotify()
		{
			_firstNotifyComplited = true;
		}

		private void SecondNotify()
		{
			_timer.Dispose();
		}


		public EmailIgnoreNotifierManger(UnitOfWork work)
		{

		}

	}
}