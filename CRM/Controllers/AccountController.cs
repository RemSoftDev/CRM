using AutoMapper;
using CRM.DAL.Entities;
using CRM.Enums;
using CRM.Managers;
using CRM.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CRM.Controllers
{
	public class AccountController : Controller
	{
		private readonly IUserManager _userManager;

		public AccountController(IUserManager userManager)
		{
			_userManager = userManager;
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
				var authTicket = new FormsAuthenticationTicket(1, user.FirstName + "|" + user.Email, DateTime.Now, DateTime.Now.AddDays(5), false, ((UserRole)user.Role).ToString());
				string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
				var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
				HttpContext.Response.Cookies.Add(authCookie);

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

		[Authorize]
		[HttpPost]
		public ActionResult LogOut()
		{
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