using log4net;
using System;
//tmpfesfsd
namespace CRM.Log
{
	public static class Logger
	{
		private static ILog _infoLog;
		private static ILog _errorLog;
		private static int row;
		private static Type type;

		static Logger()
		{
			row = 0;
			InitLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		}

		public static void InitLog(Type type)
		{
			_infoLog = LogManager.GetLogger("InfoLogger");
			_errorLog = LogManager.GetLogger("ErrorLogger");

			Logger.type = type;

			InfoLogContext.Info($"Create instance of {type}");
		}

		public static void SqlInfo(string message) => InfoLogContext.Info($"[SQL] {message}");
		public static void SqlError(string message) => ErrorLogContex.Info($"[SQL] {message}");

		private static void incrementRow()
		{
			//TODO Interconect
			row++;
		}

		public static ILog ErrorLogContex
		{
			get => _errorLog;
		}

		public static ILog InfoLogContext
		{
			get => _infoLog;
		}
	}
}
