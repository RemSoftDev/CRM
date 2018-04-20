using AutoMapper;
using CRM.Attributes;
using CRM.Enums;
using CRM.Models;
using CRM.Services;
using CRMData.Adapters;
using CRMData.Contexts;
using CRMData.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CRM.Controllers
{
    [Authenticate]
    public class LeadController : Controller
    {
        public ActionResult Index()
        {
            List<LeadViewModel> leads;
            using (BaseContext context = new BaseContext())
            {
                leads = Mapper.Map<List<Lead>, List<LeadViewModel>>(context
                    .Leads
                    .Include("Phones")
                    .Where(l => l.Customer == null)
                    .ToList());
            }
            return View(leads);
        }

        [HttpPost]
        public ActionResult Search(SearchViewModel model)
        {
            model.TableName = "Leads";

            if (!ModelState.IsValid)
            {
                return Json(new { status = "error", message = "Model is not valid!" });
            }

            var leadAdapter = new LeadAdapter();

            bool? byName = null;
            bool? byEmail = null;
            bool? byPhone = null;

            switch (model.OrderField)
            {
                case "Name":
                    byName = true;
                    break;
                case "Phone":
                    byPhone = true;
                    break;
                case "Email":
                    byEmail = true;
                    break;
                default:
                    byName = true;
                    break;
            }

            var result = leadAdapter.GetLeadsByFilter(
                model.Field, 
                model.SearchValue, 
                byName, 
                byEmail, 
                byPhone,
                model.OrderDirection.Equals("ASC"));

            var items = Mapper.Map<List<Lead>, List<LeadViewModel>>(result);
            return PartialView("_LeadsPagePartial", items);
        }

        public void CreateExpression()
        {

        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            LeadViewModel lead;
            List<string> notes;
            using (BaseContext context = ContextFactory.SingleContextFactory.Get<BaseContext>())
            {
                Lead leadDb = context.Leads.Include("Phones").FirstOrDefault(l => l.Id == id);
                lead = Mapper.Map<Lead, LeadViewModel>(leadDb);
                notes = context.Notes.Where(n => n.LeadId == leadDb.Id).Select(i => i.Text).ToList();
                lead.Notes = notes;
            }
            if (lead != null)
            {
                return View(lead);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(LeadViewModel lead,  int id)
        {
            if (ModelState.IsValid)
            {
                using (BaseContext context = ContextFactory.SingleContextFactory.Get<BaseContext>())
                {
                    Lead leadToEdit = context
                        .Leads
                        .Include("Phones")
                        .FirstOrDefault(l => l.Id == id);

                    if (leadToEdit != null)
                    {
                        leadToEdit.Email = lead.Email;
                        leadToEdit.Name = lead.Name;
                        leadToEdit
                            .Phones
                            .FirstOrDefault()
                            .PhoneNumber = lead.Phones.FirstOrDefault().PhoneNumber;

                    }
                    if (lead.Notes.Any())
                    {
                        foreach (string note in lead.Notes)
                        {
                            context.Notes.Add(new Note { LeadId = leadToEdit.Id, Text = note });
                        }
                    }
                    context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SendMessage(int id, string message)
        {
            var leadEmail = "";
            var currentUserEmail = User.Identity.Name.Split('|')[1];
            using (BaseContext context = ContextFactory.SingleContextFactory.Get<BaseContext>())
            {
                Lead lead = context.Leads.FirstOrDefault(l => l.Id == id);
                if(lead != null)
                {
                    leadEmail = lead?.Email;
                    if (lead.LeadOwner == 0)
                    {
                        var currentUser = context.Users.FirstOrDefault(u => u.Email == currentUserEmail);
                        lead.LeadOwner = currentUser.Id;
                        context.SaveChanges();
                    }
                }
            }
            if (string.IsNullOrEmpty(leadEmail))
            {
                return Json(new { status = "error" });
            }
            EmailService.SendEmail(leadEmail, "Test Message!", message);
            return Json(new { status = "success" });
        }

        [HttpGet]
        public ActionResult ConvertLead(int id)
        {
            return View(id);
        }

        [HttpPost]
        public JsonResult ConvertLead(CustomerViewModel model, int id)
        {
            var currentUserEmail = User.Identity.Name.Split('|')[1];
            LeadConvertService.Convert(model, id, currentUserEmail);

            return Json(new { status = "success" });
        }
    }
}