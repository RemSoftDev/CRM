using CRM.Enums;
using CRM.Models;
using CRM.Extentions;
using CRM.Services.Interfaces;
using CRM.DAL.Entities;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CRM.DAL.Repository;

namespace CRM.Controllers
{
    public class AccountController : Controller
    {
	    private readonly IUnitOfWork _unitOfWork;
	    private readonly IEncryptionService _encryptionService;

        public AccountController(IEncryptionService encryptionService, IUnitOfWork unitOfWork)
        {
	        _unitOfWork = unitOfWork;
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

	        User user = GetUser(model.Email, model.Password);

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

	        User user = GetUser(model.Email, model.Password);

	        if (user == null)
	        {
				_unitOfWork.UsersRepository.Create(new User
				{
					Email = model.Email,
					Password = _encryptionService.Encrypt(model.Password),
					FirstName = model.FirstName,
					LastName = model.LastName,
					Role = (int)UserRole.AdminStaff,
					UserTypeId = (int)UserType.AdminTeamMember
				});

				_unitOfWork.Save();
		        isNewUser = true;
	        }

			if (isNewUser)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

	    private User GetUser(string email, string password)
	    {
		    string encryptedPassword = _encryptionService.Encrypt(password);

		    return _unitOfWork.UsersRepository
			    .Get(u => u.Email == email && u.Password == encryptedPassword)
			    .FirstOrDefault();
		}
    }
}