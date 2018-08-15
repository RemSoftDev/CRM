using AutoMapper;
using CRM.DAL.Entities;
using CRM.DAL.Repository;
using CRM.Enums;
using CRM.Extentions;
using CRM.Managers;
using CRM.Models;
using CRM.Services.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CRM.Controllers
{
	public class AccountController : BaseController
	{
		private readonly IEmailIgnoreNotifierManger _emailIgnoreNotifier;
		private readonly IUserManager _userManager;
		private readonly IEncryptionService _encryptionService;
		private readonly IUserConnectionStorage _userConnectionStorage;

		public AccountController(
			IUserManager userManager,
			IEncryptionService encryptionService,
			IUserConnectionStorage userConnectionStorage,
			IEmailIgnoreNotifierManger emailIgnoreNotifier,
			IUnitOfWork unitOfWork)
			: base(unitOfWork)
		{
			_emailIgnoreNotifier = emailIgnoreNotifier;
			_userManager = userManager.ValidateNotDefault(nameof(userManager));
			_encryptionService = encryptionService.ValidateNotDefault(nameof(encryptionService));
			_userConnectionStorage = userConnectionStorage.ValidateNotDefault(nameof(userConnectionStorage));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Login(LoginViewModel model, string returnUrl)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			User user = _userManager.GetUser(model.Email, model.Password);

			if (user != null)
			{
				FormsAuthentication.SetAuthCookie(model.Email, false);

				#region claim

				//string jsonUserInfo = JsonConvert.SerializeObject(new AutenticateUser()
				//{
				//    Id = user.Id,
				//    FirstName = user.FirstName,
				//    Email = user.Email,
				//    Role = user.Role
				//});
				//var authTicket = new FormsAuthenticationTicket(1, user.FirstName, DateTime.Now, DateTime.Now.AddDays(5), false, jsonUserInfo);

				#endregion

				var authTicket = new FormsAuthenticationTicket(1, $"{user.Id}|{user.FirstName}|{user.Email}", DateTime.Now, DateTime.Now.AddDays(5), false, ((UserRole)user.Role).ToString());
				string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
				var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
				HttpContext.Response.Cookies.Add(authCookie);

				// insert userId into list of online users
				_userConnectionStorage.AddUser(user.Id);

				return RedirectToAction("Index", "Lead");
			}
			else
			{
				AddError("Invalid login attempt.");
				return View(model);
			}
		}

		[HttpGet]
		[AllowAnonymous]
		public ActionResult Login()
		{
			return View();
		}

		[HttpGet]
		[AllowAnonymous]
		public ActionResult Test()
		{
			_emailIgnoreNotifier.Start();
			return View(nameof(Login));
		}

		[Authorize]
		[HttpPost]
		public ActionResult LogOut()
		{
			// remove userId from list of online users
			var userCreds = User.GetCurrentUserCreads();
			if (userCreds != null)
			{
				_userConnectionStorage.RemoveUser(userCreds.Id);
			}

			FormsAuthentication.SignOut();
			return RedirectToAction("Login", "Account");
		}

		[HttpGet]
		public ActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{

				var user = Mapper.Map<RegisterViewModel, User>(model);

				var result = _userManager.Create(user, model.Password);

				if (result.Succeeded)
				{
					return RedirectToAction("Login", "Account");
				}

				AddErrors(result);
			}

			return View(model);
		}

		private void AddErrors(IdentityResult result)
		{
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError("", error);
			}
		}

		private void AddError(string errorMessage)
		{
			ModelState.AddModelError("", errorMessage);
		}
	}
}