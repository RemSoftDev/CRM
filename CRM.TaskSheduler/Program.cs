using CRM.DAL.Repository;
using CRM.Models;
using CRM.Services;
using System;
using System.IO;
using System.Reflection;

namespace CRM.TaskSheduler
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello, wolrd!");

			var path = GetLogFilePath();
			var logs = GetAllLogsInFile(path);

			var email = new EmailViewModel
			{
				To = "Yurii.Moroziuk@hotmail.com",
				Subject = $"CRM log {DateTime.Now}",
				Body = string.Join(Environment.NewLine, logs)
			};

			using (var uow = new UnitOfWork())
			{
				var emailService = new EmailService(uow);
				emailService.SendEmail(email, new UserViewModel());
			}
		}

		private static string[] GetAllLogsInFile(string path)
		{
			if (File.Exists(path))
			{
				return File.ReadAllLines(path);
			}

			return null;
		}

		private static string GetLogFilePath()
		{
			string currentDirPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			string relativaPath = @"..\..\..\CRM\Logs\CRM_LOG.txt";
			return Path.GetFullPath(Path.Combine(currentDirPath, relativaPath));
		}
	}
}
