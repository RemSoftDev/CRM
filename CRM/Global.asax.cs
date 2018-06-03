using CRM.App_Start;
using CRM.AutoMapper;
using CRM.DAL.Repository;
using CRM.Log;
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
	/// <summary>
	/// Class get access to IKernel
	/// </summary>
	public static class Kernel
	{
		public static IKernel GetKernel { get; set; }
	}

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

		protected override void OnApplicationStopped()
		{
			Logger.InfoLogContext.Error("Web App was stopped or down from ", new Exception());
		}


		protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
		{
			var authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
			if (authCookie != null)
			{
				FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
				if (authTicket != null && !authTicket.Expired)
				{
					#region version autentication using Claim

					//var jsonUserInfo = JsonConvert.DeserializeObject<AutenticateUser>(authTicket.UserData, new JsonSerializerSettings()
					//{
					//    Error = delegate (object jsonSender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
					//    {
					//        args.ErrorContext.Handled = true;
					//    },

					//});

					//if (jsonUserInfo != null)
					//{

					//    var identity = new FormsIdentity(authTicket);
					//    identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, jsonUserInfo.Id.ToString()));
					//    identity.AddClaim(new Claim(ClaimTypes.Email, jsonUserInfo.Email));

					//    HttpContext.Current.User = new GenericPrincipal(identity, new string[] { ((UserRole)jsonUserInfo.Role).ToString() });
					//}

					#endregion

					var roles = authTicket.UserData.Split(',');
					HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(new FormsIdentity(authTicket), roles);
				}
			}
		}

		protected override IKernel CreateKernel()
		{
			var kernel = new StandardKernel();
			CRM.Kernel.GetKernel = kernel;

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
				.InThreadScope();

			kernel.Bind<IEmailService>()
				.To<EmailService>()
				.InSingletonScope();

			kernel.Bind<ILeadConvertService>()
				.To<LeadConvertService>()
				.InSingletonScope();

			kernel.Bind<IUserManager>()
				.To<UserManager>()
				.InSingletonScope();

			kernel.Bind<ILeadManager>()
				.To<LeadManager>()
				.InSingletonScope();

			kernel.Bind<IUserConnectionStorage>()
				.To<UserConnectionStorage>()
				.InSingletonScope();

			kernel.Bind<PhoneService>()
				.ToSelf()
				.InSingletonScope();
		}


	}
}
