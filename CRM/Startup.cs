using ASquare.WindowsTaskScheduler;
using ASquare.WindowsTaskScheduler.Models;
using CRM.Hubs;
using CRM.Services.Interfaces;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Owin;
using System;
using System.IO;
using System.Web;

[assembly: OwinStartup(typeof(CRM.Startup))]

namespace CRM
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			var resolver = new NinjectSignalRDependencyResolver(Kernel.GetKernel);

			// Add binding for SignalR
			Kernel.GetKernel.Bind(typeof(IHubConnectionContext<dynamic>)).ToMethod(context =>
					resolver.Resolve<IConnectionManager>().GetHubContext<PhoneHub>().Clients
						).WhenInjectedInto<IUserConnectionStorage>();

			app.MapSignalR(new HubConfiguration()
			{
				Resolver = resolver
			});

			NewMethod();
		}

		private static void NewMethod()
		{
			var pathToBat = $"{HttpRuntime.AppDomainAppPath}EmailSanderTask.bat";

			BuildBatFileIfNotExist(pathToBat);

			SchedulerResponse response = WindowTaskScheduler
			 .Configure()
			 .CreateTask("EmailSanderTask", pathToBat)
			 .RunDaily()
			 .RunEveryXMinutes(10)
			 .RunDurationFor(new TimeSpan(18, 0, 0))
			 .SetStartDate(DateTime.Now)
			 .SetStartTime(new TimeSpan(8, 0, 0))
			 .Execute();

			var a = response.IsSuccess;
		}

		private static void BuildBatFileIfNotExist(string pathToBat)
		{
			if (File.Exists(pathToBat)) return;

			using (StreamWriter sw = File.CreateText(pathToBat))
			{
				sw.WriteLine($"start /d {GetPathForTaskShedulerApp()}");
			}
		}

		private static string GetPathForTaskShedulerApp()
		{
			var currentDir = HttpRuntime.AppDomainAppPath;
			string path = string.Empty;
#if DEBUG
			path = @"..\CRM.TaskSheduler\bin\Debug CRM.TaskSheduler.exe";
#else

#endif

			return Path.GetFullPath(Path.Combine(currentDir, path));
		}
	}

}
