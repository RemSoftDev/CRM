﻿using CRM.Hubs;
using CRM.Services.Interfaces;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Owin;

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

		}



	}

}
