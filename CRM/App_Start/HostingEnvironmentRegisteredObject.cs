using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace CRM.App_Start
{
	public class HostingEnvironmentRegisteredObject : IRegisteredObject
	{
		// this is called both when shutting down starts and when it ends
		public void Stop(bool immediate)
		{
			if (immediate)
				return;

			Log.Logger.Error("Web app was down", new Exception());

			// shutting down code here
			// there will about Shutting down time limit seconds to do the work
		}
	}
}