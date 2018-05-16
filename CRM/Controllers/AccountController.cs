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
using CRM.Hubs;

namespace CRM.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEncryptionService encryptionService;
        private readonly IUserConnectionStorage userConnectionStorage;

        public AccountController(IEncryptionService encryptionService, IUserConnectionStorage userConnectionStorage)
        {
            this.encryptionService = encryptionService.ValidateNotDefault(nameof(encryptionService));
            this.userConnectionStorage = userConnectionStorage;
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