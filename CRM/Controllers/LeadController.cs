using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    public class LeadController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateLead()
        {
            return View();
        }    
    }
}