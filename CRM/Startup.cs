using CRM.Hubs;
using CRM.Log;
using CRM.Services.Interfaces;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartup(typeof(CRM.Startup))]

namespace CRM
{
    public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			var resolver = new NinjectSignalRDependencyResolver(Kernel.GetKernel);

            // Add binding for SignalR
            Kernel.GetKernel.Bind<IHubConnectionContext<dynamic>>().ToMethod(context =>
                    resolver.Resolve<IConnectionManager>().GetHubContext<MainHub>().Clients
                        ).WhenInjectedInto<IUserConnectionStorage>();

            app.MapSignalR(new HubConfiguration()
			{
				Resolver = resolver
			});

            Kernel.GetKernel.Bind<IHubContext>()
                .ToMethod(e => GlobalHost.ConnectionManager.GetHubContext<MainHub>());

            try
            {
				throw new Exception();
			}
			catch (Exception exp)
			{
				Logger.ErrorLogContex.Error("something wrong",exp);
			}
			Logger.InfoLogContext.Info("Web app start");
		}
	}
}
