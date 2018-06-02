using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using CRM.DAL.Repository;
using CRM.Models;
using CRM.Services;

namespace CRM.Modules
{
	public class EmailSchedulerModule: IHttpModule
	{
		private readonly IEmailService _emailService;
		private readonly IUnitOfWork _unitOfWork;
		private Timer _timer;
		private readonly long interval = 600000; //30 секунд
		static object synclock = new object();
		private DateTime _startTimer;

		//public EmailSchedulerModule(IEmailService emailService, IUnitOfWork unitOfWork)
		//{
		//	_emailService = emailService;
		//	_unitOfWork = unitOfWork;
		//}
		public void Init(HttpApplication context)
		{
			_timer = new Timer(SendEmail, null, 0, interval);
			_startTimer = DateTime.Now.AddMinutes(10);
		}

		private void SendEmail(object obj)
		{
			lock (synclock)
			{
				if (DateTime.Now >= _startTimer)
				{
					var path = GetLogFilePath();
					var logs = GetAllLogsInFile(path);


					var email = new EmailViewModel
					{
						To = "Yurii.Moroziuk@hotmail.com",
						Subject = $"CRM log {DateTime.Now}",
						Body = logs
					};

					var uow = new UnitOfWork();
					
					var emailService = new EmailService(uow);
					emailService.SendEmail(email);
				}
			}
		}

		private string GetAllLogsInFile(string path)
		{
			lock (synclock)
			{
				string logs = null;

				try
				{
					using (StreamReader sr = new StreamReader(path))
					{
						logs = sr.ReadToEnd();
					}
				}
				catch
				{

				}
				

				return logs;
			}
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