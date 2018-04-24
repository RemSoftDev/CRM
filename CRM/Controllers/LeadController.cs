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
using System.Data.Entity;
using CRM.Extentions;

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
                var leads1 = context
                    .Leads
                    .Include(e => e.Phones)
                    .Where(l => l.Customer == null)
                    .ToList();


                leads = Mapper.Map<List<Lead>, List<LeadViewModel>>(leads1);
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

        public ActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            LeadViewModel lead;

            using (BaseContext context = ContextFactory.SingleContextFactory.Get<BaseContext>())
            {
                Lead leadDb = context.Leads.Include(e => e.Phones).FirstOrDefault(l => l.Id == id);
                lead = Mapper.Map<Lead, LeadViewModel>(leadDb);

                List<Note> leadNotes = context.Notes.Where(n => n.LeadId == leadDb.Id).ToList();
                lead.Notes = Mapper.Map<List<Note>, List<NoteViewModel>>(leadNotes);
            }
            if (lead != null)
            {
                return View(lead);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(LeadViewModel lead, List<string> note)
        {
            if (ModelState.IsValid)
            {
                using (BaseContext context = ContextFactory.SingleContextFactory.Get<BaseContext>())
                {
                    Lead leadToEdit = context
                        .Leads
                        .Include(e => e.Phones)
                        .FirstOrDefault(l => l.Id == lead.Id);

                    if (leadToEdit != null)
                    {
                        leadToEdit.Email = lead.Email;
                        leadToEdit.Name = lead.Name;
                        leadToEdit.Discipline = lead.Discipline;
                        leadToEdit.AgeGroup = lead.AgeGroup;
                        leadToEdit.Status = lead.Status;
                        leadToEdit.StatusNotes = lead.StatusNotes;
                        leadToEdit.IssueRaised = lead.IssueRaised;

                        var firstPhone = leadToEdit
                            .Phones
                            .FirstOrDefault();

                        if(firstPhone == null)
                        {
                            leadToEdit.Phones.Add(new Phone()
                            {
                                PhoneNumber = lead.Phones.FirstOrDefault().PhoneNumber,
                                Type = new DPhoneType()
                                {
                                    TypeName = "HomePhone"
                                }
                            });
                        }
                        else
                        {
                            firstPhone.PhoneNumber = lead.Phones.FirstOrDefault().PhoneNumber;
                        }

                    }

                    if (lead.Notes.Any())
                    {
                        foreach (NoteViewModel reNewNote in lead.Notes)
                        {
                            Note oldNote = context.Notes.FirstOrDefault(e => e.Id == reNewNote.Id);
                            if (oldNote != null)
                                oldNote.Text = reNewNote.Text;
                        }
                    }

                    if (note != null && note.Any())
                    {
                        context.Notes.AddRange(note.Select(e => new Note() { Text = e, LeadId = lead.Id }));
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
            var userCreads = User.GetCurrentUserCreads();
            using (BaseContext context = ContextFactory.SingleContextFactory.Get<BaseContext>())
            {
                Lead lead = context.Leads.FirstOrDefault(l => l.Id == id);
                if (lead != null)
                {
                    leadEmail = lead?.Email;
                    if (lead.LeadOwner == 0)
                    {
                        var currentUser = context.Users.FirstOrDefault(u => u.Email == userCreads.Email);
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
        public ActionResult Conversation(int id)
        {
            string leadEmail = "";
            using (BaseContext context = ContextFactory.SingleContextFactory.Get<BaseContext>())
            {
                Lead lead = context.Leads.FirstOrDefault(l => l.Id == id);
                if (lead != null)
                {
                    leadEmail = lead?.Email;
                }
            }

            var mailings = EmailService.GetMailings(leadEmail);

            return View(mailings);
        }

        [HttpGet]
        public ActionResult ConvertLead(int id)
        {
            var customer = new CustomerViewModel();
            customer.Address.Add(new AddressViewModel());
            customer.Notes.Add(new NoteViewModel());

            using (var context = new BaseContext())
            {
                var currentLead = context.Leads
                    .Include(e => e.Phones)
                    .FirstOrDefault(e => e.Id == id);
 
                customer.Email = currentLead.Email;
                customer.Phones = Mapper.Map<List<PhoneViewModel>>(currentLead.Phones);
            }

            return View(new LeadConvertViewModel()
            {
                Id = id,
                NewCustomer = customer
            });
        }

        [HttpPost]
        public ActionResult ConvertLead(LeadConvertViewModel model, List<AddressViewModel> newAddress, List<PhoneViewModel> newPhones)
        {
            model.NewCustomer.Address.AddRange(newAddress);
            model.NewCustomer.Phones.AddRange(newPhones);

            var userCreads = User.GetCurrentUserCreads();

            LeadConvertService.Convert(model, userCreads.Email);

            return RedirectToAction("Index", "Customer");
        }
    }
}