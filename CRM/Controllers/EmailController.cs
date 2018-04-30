using AutoMapper;
using CRM.Attributes;
using CRM.Extentions;
using CRM.Interfaces;
using CRM.Models;
using CRM.Services;
using CRMData.Contexts;
using CRMData.Entities;
using System;
using System.Linq;
using System.Web.Mvc;

namespace CRM.Controllers
{
    [Authenticate]
    public class EmailController : Controller
    {
        [HttpPost]
        public ActionResult SendMessage(int id, string message)
        {
            IUser user = GetUserModel(id);

            if (string.IsNullOrEmpty(user.Email))
            {
                return Json(new { status = "error" });
            }
            if (user is LeadViewModel && user != null)
            {
                SetLeadOwner(user);
            }

            EmailService.SendEmail(new EmailViewModel(user.Email, "CRM Message!", message), user);

            return Json(new { status = "success" });
        }

        public ActionResult Conversation(int id)
        {
            IUser user = GetUserModel(id);
            var mailings = EmailService.LoadMails(user);

            ViewBag.Id = id;
            ViewBag.UserType = (user is LeadViewModel) ? "Lead" : "Customer";

            return View(mailings);
        }

        [HttpPost]
        public ActionResult GetEmails(int id, string type)
        {         
            IUser user = GetUserModel(id, type);          

            var mailings = EmailService.GetMailings(GetLastReceivedDate(user), user);

            return Json(new { mailings });
        }

        private void SetLeadOwner(IUser model)
        {
            using (BaseContext context = ContextFactory.SingleContextFactory.Get<BaseContext>())
            {
                Lead lead = context.Leads.FirstOrDefault(l => l.Id == model.Id);
                if (lead != null)
                {
                    if (lead.LeadOwner == 0)
                    {
                        var userCreads = User.GetCurrentUserCreads();
                        var currentUser = context.Users.FirstOrDefault(u => u.Email == userCreads.Email);
                        lead.LeadOwner = currentUser.Id;
                        context.SaveChanges();
                    }
                }
            }
        }

        private User GetCustomerModel(int id)
        {
            User customerModel;

            using (BaseContext context = ContextFactory.SingleContextFactory.Get<BaseContext>())
            {
                customerModel = context.Users.FirstOrDefault(l => l.Id == id);
            }
            return customerModel;
        }

        private Lead GetLeadModel(int id)
        {
            Lead leadModel;

            using (BaseContext context = ContextFactory.SingleContextFactory.Get<BaseContext>())
            {
                leadModel = context.Leads.FirstOrDefault(l => l.Id == id);
            }
            return leadModel;
        }

        private DateTime? GetLastReceivedDate(IUser user)
        {
            DateTime? date;
            if (user is LeadViewModel)
            {
                using (BaseContext context = new BaseContext())
                {
                    date = context.Emails
                        .OrderByDescending(e => e.Id)
                        .FirstOrDefault(e => e.LeadId == user.Id)?
                        .SentDate;
                }
            }
            else
            {
                using (BaseContext context = new BaseContext())
                {
                    date = context.Emails.OrderByDescending(e => e.Id)
                        .FirstOrDefault(e => e.UserId == user.Id)?
                        .SentDate;
                }
            }
            return date;
        }

        private IUser GetUserModel(int id, string type = "")
        {
            IUser user;
            if (type == "lead" || HttpContext.Request.UrlReferrer.AbsolutePath.Contains("Lead"))
            {
                Lead lead = GetLeadModel(id);
                user = Mapper.Map<Lead, LeadViewModel>(lead);
            }
            else
            {
                User customer = GetCustomerModel(id);
                user = Mapper.Map<User, UserViewModel>(customer);
            }
            return user;
        }
    }
}