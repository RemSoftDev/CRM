using CRM.Log;
using System;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
	public class TaskShedulerController : Controller
	{
		private WindowTaskSchedulerBuilder _builder;

		public TaskShedulerController()
		{
			_builder = new WindowTaskSchedulerBuilder();
		}
		// GET: TaskSheduler
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult StopLogTask()
		{
			_builder.RemoveLogTask();
			return View("Index");
		}
		public ActionResult StartLogTask()
		{

			_builder.SetAppDomainPath(HttpRuntime.AppDomainAppPath)
				.SetBatFileWithPath($"{HttpRuntime.AppDomainAppPath}EmailSanderTask.bat")
				.SetTaskDuration(new TimeSpan(23, 0, 0))
				.SetStartTime(new TimeSpan(0, 10, 0))
				.Build();

			return View("Index");
		}
	}
}