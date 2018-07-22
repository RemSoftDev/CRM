using AutoMapper;
using CRM.DAL.Entities;
using CRM.DAL.Repository;
using CRM.Enums;
using CRM.Extentions;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        //[HttpGet]
        public ActionResult BanerLink(string term)
        {
            var result = _unitOfWork.ConditionsRepository
                .Get(e => e.Name.Equals(term, StringComparison.InvariantCultureIgnoreCase))
                .ToList();

            return Json(result.Select(e => e.Description).ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}