using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using crmStart.Models;

namespace crmStart.Controllers
{
    public class HomeController : Controller
    {
        private CRMContext cRMContext { get; set; }

        public HomeController()
        {
            this.cRMContext = new CRMContext();
            this.cRMContext.Customers.Add(new Customer());
            this.cRMContext.SaveChanges();
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View((object)"Show Form"); // TODO make a special form
        }

        [HttpPost]
        public ActionResult Index(User user)
        {
            if (ModelState.IsValid)
            {
                User newUser = new User();
                newUser.Password = MakeEncryption(user.Password, user.Salt); // TODO what to do with salt
                return View(newUser);
            }
            else
                return View((object)"User model invalid");
        }

        private string MakeEncryption(string password, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password + salt);
            SHA512 shaM = new SHA512Managed();
            MD5 mD5 = MD5.Create();
            byte[] result = mD5.ComputeHash(shaM.ComputeHash(bytes));
            string output = Encoding.UTF8.GetString(result);
            return output;
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}