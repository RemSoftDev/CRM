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
using AutoMapper;

namespace CRM.Controllers
{
	public class AccountController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IUserManager _userManager;
		private readonly IEncryptionService _encryptionService;

		public AccountController(
			IEncryptionService encryptionService,
			IUnitOfWork unitOfWork,
			IUserManager userManager)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_encryptionService = encryptionService.ValidateNotDefault(nameof(encryptionService));
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
				ModelState.AddModelError("", "Invalid login attempt.");
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
		public ActionResult Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				////ToDO Create Mapper
				var user = new User
				{
					Email = model.Email,
					Password = model.Password,
					FirstName = model.FirstName,
					LastName = model.LastName,
					Role = (int)UserRole.AdminStaff,
					UserTypeId = (int)UserType.AdminTeamMember
				};

				//var user = Mapper.Map<RegisterViewModel, User>(model);
				//user.Role = (int) UserRole.AdminStaff;
				//user.UserTypeId = (int) UserType.AdminTeamMember;

				var result = _userManager.Create(user, model.Password);

				if (result.Succeeded)
				{
					return RedirectToAction("Login", "Account"); ;
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
	}
}