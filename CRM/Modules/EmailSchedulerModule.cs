using CRM.DAL.Repository;
using CRM.Models;
using CRM.Services;
using System;
using System.IO;
using System.Threading;
using System.Web;

namespace CRM.Modules
{
	public class EmailSchedulerModule : IHttpModule
	{
		private IEmailService _emailService;
		private Timer _timer;
		private readonly long interval = 600000;
		static object synclock = new object();
		private DateTime _startTimer;
		private static bool _initialised;

		public void Init(HttpApplication context)
		{
			lock (synclock)
			{
				if (!_initialised)
				{
					var unitOfWork = new UnitOfWork();
					_emailService = new EmailService(unitOfWork);

					_timer = new Timer(SendEmail, null, 0, interval);
					_startTimer = DateTime.Now.AddMinutes(10);
					_initialised = true;
				}
			}
		}

		private void SendEmail(object obj)
		{
			if (DateTime.Now >= _startTimer)
			{
				var path = GetLogFilePath();
				var logs = GetAllLogsInFile(path);

				if (logs == null) return;

				var email = new EmailViewModel
				{
					To = "csharpcrm@gmail.com",
					Subject = $"CRM log {DateTime.Now}",
					Body = logs
				};

				_emailService.SendEmail(email);
			}
		}

		private string GetAllLogsInFile(string path)
		{
			string logs = null;

			try
			{

				logs = File.ReadAllText(path);

			}
			catch (Exception exp)
			{
				var msd = exp.Message;
			}


			return logs;
		}

		private string GetLogFilePath()
		{
			string currentDirPath = HttpRuntime.AppDomainAppPath;
			string relativaPath = @"Logs\CRM_LOG.txt";
			return Path.GetFullPath(Path.Combine(currentDirPath, relativaPath));
		}

		public void Dispose()
		{ }
	}
}