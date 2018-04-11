using CRM.Models;
using CRM.Services;
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
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            User user;
            using (BaseContext context = new BaseContext())
            {
                user = context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
            }
                
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(model.Email, false);

                var authTicket = new FormsAuthenticationTicket(1, user.Email, DateTime.Now, DateTime.Now.AddMinutes(20), false, user.Role);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Register(RegisterViewModel model)
        {
            bool isNewUser = false;
            using (BaseContext context = new BaseContext())
            {
                User user = context.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
                if (user == null)
                {
                    context.Users.Add(new User { Email = model.Email, Password = EncryptionService.Encrypt(model.Password) });
                    context.SaveChanges();
                    isNewUser = true;
                }
            }

            if (isNewUser)
            {
                return Json(new { success = true, url = "/Account/Login" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false, model }, JsonRequestBehavior.AllowGet);
        }
    }
}