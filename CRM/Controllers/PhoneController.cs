using CRM.Extentions;
using CRM.Services.Interfaces;
using CRMData.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CRM.Controllers
{
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
        public ActionResult Index()
        {
            List<int> usersId = _userConnectionStorage.GetCurrentUsersExceptUser(User.GetCurrentUserCreads().Id);

            var users = _unitOfWork.Context.Users
                .Where(e => usersId.Contains(e.Id))
                //.Select(e => new SelectListItem()
                //{
                //    Text = e.FirstName,
                //    Value = e.Id.ToString(),
                //    Selected = false
                //})
                .ToList();

            return View(new SelectList(users, "Id", "FirstName"));
        }

        
        public ActionResult GetF()
        {
            List<int> usersId = _userConnectionStorage.GetCurrentUsersExceptUser(User.GetCurrentUserCreads().Id);

            var users = _unitOfWork.Context.Users
                .Where(e => usersId.Contains(e.Id))
                //.Select(e => new SelectListItem()
                //{
                //    Text = e.FirstName,
                //    Value = e.Id.ToString(),
                //    Selected = false
                //})
                .ToList();

            return PartialView("Drop", new SelectList(users, "Id", "FirstName"));
        }

        public ActionResult PhoneCall()
        {
            return View();
        }
    }
}