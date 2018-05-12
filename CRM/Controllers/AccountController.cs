using CRM.Enums;
using CRM.Models;
using CRM.Services;
using CRM.Extentions;
using CRM.Services.Interfaces;
using CRMData.Contexts;
using CRMData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CRM.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEncryptionService encryptionService;

        public AccountController(IEncryptionService encryptionService)
        {
            this.encryptionService = encryptionService.ValidateNotDefault(nameof(encryptionService));
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
            bool isNewUser = false;
            using (BaseContext context = new BaseContext())
            {
                string encryptedPassword = encryptionService.Encrypt(model.Password);
                User user = context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == encryptedPassword);
                if (user == null)
                {
                    context.Users.Add(new User {
                        Email = model.Email,
                        Password = encryptedPassword,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Role = (int)UserRole.AdminStaff,
                        UserTypeId = (int)UserType.AdminTeamMember});

                    context.SaveChanges();
                    isNewUser = true;
                }
            }

            if (isNewUser)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }
    }
}