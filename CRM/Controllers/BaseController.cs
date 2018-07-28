using CRM.DAL.Repository;
using CRM.Enums;
using CRM.Extentions;
using System.Linq;
using System.Web.Mvc;

namespace CRM.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IUnitOfWork _unitOfWork;

        public BaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork.ValidateNotDefault(nameof(unitOfWork));
        }

        [ChildActionOnly]
        public ActionResult PopUp()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userCreads = User.GetCurrentUserCreads();

                if (_unitOfWork.UsersRepository.FindById(userCreads.Id).UserTypeId == (int)UserType.AdminTeamMember)
                {
                    return PartialView("PopUp");
                }
                return new EmptyResult();
            }
            return new EmptyResult();
        }

        /// <summary>
        /// ATENTION. We allow for all users make request to this method
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BanerLink(string term)
        {
            var result = _unitOfWork.ConditionsRepository
                .Get(e => e.Name.Contains(term))
                .ToList();

            return Json(new
            {
                message = result.Select(e => new
                {
                    value = e.Name,
                    lable = e.Description
                })
            }, JsonRequestBehavior.AllowGet);
        }
    }
}