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
using CRM.Hubs;

namespace CRM.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEncryptionService encryptionService;
        private readonly IUserConnectionStorage userConnectionStorage;
	public class AccountController : Controller
	{
		private readonly IUserManager _userManager;

        public AccountController(IEncryptionService encryptionService, IUserConnectionStorage userConnectionStorage)
        {
            this.encryptionService = encryptionService.ValidateNotDefault(nameof(encryptionService));
            this.userConnectionStorage = userConnectionStorage;
        }
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            User user;
            using (BaseContext context = new BaseContext())
            {
                string encryptedPassword = encryptionService.Encrypt(model.Password);
                user = context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == encryptedPassword);
            }
                
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(model.Email, false);

                //string jsonUserInfo = JsonConvert.SerializeObject(new AutenticateUser()
                //{
                //    Id = user.Id,
                //    FirstName = user.FirstName,
                //    Email = user.Email,
                //    Role = user.Role
                //});
                //var authTicket = new FormsAuthenticationTicket(1, user.FirstName, DateTime.Now, DateTime.Now.AddDays(5), false, jsonUserInfo);

                var authTicket = new FormsAuthenticationTicket(1, $"{user.Id}|{user.FirstName}|{user.Email}", DateTime.Now, DateTime.Now.AddDays(5), false, ((UserRole)user.Role).ToString());
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);

                // insert userId into list of online users
                userConnectionStorage.AddUser(user.Id);

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
            // remove userId from list of online users
            var userCreds = User.GetCurrentUserCreads();
            if (userCreds != null)
            {
                userConnectionStorage.RemoveUser(userCreds.Id);
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