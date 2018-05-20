using CRM.Attributes;
using CRM.DAL.Entities;
using CRM.DAL.Repository;
using CRM.Extentions;
using CRM.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CRM.Controllers
{
    [Authenticate]
    public class PhoneController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserConnectionStorage _userConnectionStorage;

        public PhoneController(IUnitOfWork unitOfWork, IUserConnectionStorage userConnectionStorage)
        {
            _unitOfWork = unitOfWork.ValidateNotDefault(nameof(unitOfWork));
            _userConnectionStorage = userConnectionStorage.ValidateNotDefault(nameof(userConnectionStorage));
        }

        // GET: Phone
        [HttpGet]
        public ActionResult Index()
        {
            List<User> users = GetCurrentUsersExceptUser();

            return View(new SelectList(users, "Id", "FirstName"));
        }

        /// <summary>
        /// Refresh dropdown list
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        public ActionResult RefreshDropDown()
        {
            List<User> users = GetCurrentUsersExceptUser();

            return PartialView("_Drop", new SelectList(users, "Id", "FirstName"));
        }

        public ActionResult PhoneCall()
        {
            return View();
        }

        /// <summary>
        /// Get logged users without current
        /// </summary>
        private List<User> GetCurrentUsersExceptUser()
        {
            List<int> usersId = _userConnectionStorage.GetCurrentUsersExceptUser(User.GetCurrentUserCreads().Id);

            return _unitOfWork.Context.Users
                .Where(e => usersId.Contains(e.Id))
                .ToList();
        }
    }
}