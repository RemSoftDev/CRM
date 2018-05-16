using CRM.App_Start;
using CRM.AutoMapper;
using CRM.DAL.Repository;
using CRM.Managers;
using CRM.Services;
using CRM.Services.Interfaces;
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
				.To<UnitOfWork>()
				.InSingletonScope();

			kernel.Bind<IEmailService>()
				.ToMethod(e => new EmailService(kernel.Get<IUnitOfWork>()));

			kernel.Bind<ILeadConvertService>()
				.ToMethod(e => new LeadConvertService(kernel.Get<IUnitOfWork>()));

			kernel.Bind<IUserManager>()
				.ToMethod(e => new UserManager(kernel.Get<IUnitOfWork>(), kernel.Get<IEncryptionService>()));

			kernel.Bind<ILeadManager>()
				.ToMethod(e => new LeadManager(kernel.Get<IUnitOfWork>()));
		}
	}
}
