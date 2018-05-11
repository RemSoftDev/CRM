using CRM.App_Start;
using CRM.AutoMapper;
using CRM.Services;
using CRM.Services.Interfaces;
using CRMData.Repository;
using Ninject;
using Ninject.Web.Common.WebHost;
using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace CRM
{
	public class MvcApplication : NinjectHttpApplication
	{
		protected override void OnApplicationStarted()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			//GlobalFilters.Filters.Add(new LogHistoryAttribute());

			AutoMapperConfiguration.Configure();
		}

		protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
		{
			var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
			if (authCookie != null)
			{
				FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
				if (authTicket != null && !authTicket.Expired)
				{
					var roles = authTicket.UserData.Split(',');
					HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new FormsIdentity(authTicket), roles);
				}
			}
		}

		protected override IKernel CreateKernel()
		{
			var kernel = new StandardKernel();

			Register(kernel);

			return kernel;
		}

		private void Register(IKernel kernel)
		{
			kernel.Bind<IEncryptionService>()
				.ToMethod(e => new EncryptionService(ConfigProvider.EncryptionKey))
				.InSingletonScope();

			kernel.Bind<IUnitOfWork>()
				.ToMethod(e => new UnitOfWork())
				.InSingletonScope();
		}
	}
}
