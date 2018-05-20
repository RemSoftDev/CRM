using CRM.Hubs;
using CRM.Services.Interfaces;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Ninject;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;

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

    /// <summary>
    /// Resolve dependency is SignalR connection
    /// </summary>
    internal class NinjectSignalRDependencyResolver : DefaultDependencyResolver
    {
        private readonly IKernel _kernel;
        public NinjectSignalRDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        public override object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType) ?? base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType).Concat(base.GetServices(serviceType));
        }
    }
}
