using log4net;
using System.Diagnostics;

namespace CRM.DAL.Log4net
{
	public static class Log
	{
		private static readonly log4net.ILog _log;
		static Log()
		{
			GlobalContext.Properties["LogFileName"] = @"C:\\file1"; //log file path
			log4net.Config.XmlConfigurator.Configure();

			_log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		}

		public static void Info(string message)
		{
			_log.Info(message);
			Debug.Write(message);
		}
	}
}
