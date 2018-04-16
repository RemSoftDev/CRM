using AutoMapper;
using CRM.Attributes;
using CRM.Models;
using CRM.Services;
using CRMData.Contexts;
using CRMData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                leads = Mapper.Map<List<Lead>, List<LeadViewModel>>(context.Leads.Where(l => l.LeadId > 0).ToList());
            }
            return View(leads);
        }

        [HttpPost]
        public ActionResult Search(SearchModel model)
        {
            model.TableName = "Leads";

            if (!ModelState.IsValid)
            {
                return Json(new { status = "error", message = "Model is not valid!" });
            }
            
            var items = Mapper.Map<List<Lead>, List<LeadViewModel>>(SearchService<Lead>.Search(model));
            return PartialView("_LeadsPagePartial", items);
        }

        public ActionResult Create()
        {
            return View();
        }    

        [HttpGet]
        public ActionResult Edit(string phone)
        {
            LeadViewModel lead;
            List<string> notes;
            using (BaseContext context = new BaseContext())
            {
                Lead leadDb = context.Leads.FirstOrDefault(l => l.Phone == phone);
                lead = Mapper.Map<Lead, LeadViewModel>(leadDb);
                notes = context.Notes.Where(n => n.LeadId == leadDb.LeadId).Select(i => i.NoteValue).ToList();
                lead.Notes = notes;
            }
            if(lead != null)
            {
                return View(lead);
            }
            return RedirectToAction("Index"); 
        }

        [HttpPost]
        public ActionResult Edit(LeadViewModel lead,  string oldPhone)
        {
            if (ModelState.IsValid)
            {
                using (BaseContext context = new BaseContext())
                {
                    Lead leadToEdit = context.Leads.FirstOrDefault(l => l.Phone == oldPhone);
                    if(leadToEdit != null)
                    {
                        leadToEdit.Email = lead.Email;
                        leadToEdit.Name = lead.Name;
                        leadToEdit.Phone = lead.Phone;
                    }
                    if (lead.Notes.Any())
                    {
                        foreach(string note in lead.Notes)
                        {
                            context.Notes.Add(new Note {LeadId = leadToEdit.LeadId, NoteValue =  note});
                        }                      
                    }
                    context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SendMessage(string oldPhone, string message)
        {
            var leadEmail = "";
            var currentUserEmail = User.Identity.Name.Split('|')[1];
            using (BaseContext context = new BaseContext())
            {
                Lead lead = context.Leads.FirstOrDefault(l => l.Phone == oldPhone);
                if(lead != null)
                {
                    leadEmail = lead?.Email;
                    if(lead.LeadOwner == 0)
                    {
                        var currentUser = context.Users.FirstOrDefault(u => u.Email == currentUserEmail);
                        lead.LeadOwner = currentUser.UserId;
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
    }
}