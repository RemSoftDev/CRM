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
using System;

namespace CRM.Controllers
{
    [Authenticate]
    public class LeadController : Controller
    {
        public ActionResult Index()
        {
            List<LeadViewModel> leadsView;
            using (BaseContext context = new BaseContext())
            {
                var leadsNotConverted = context.Leads
                    .Include(e => e.Phones)
                    .Where(l => !l.IsConverted)
                    .ToList();

                leadsView = Mapper.Map<List<Lead>, List<LeadViewModel>>(leadsNotConverted);
            }
            return View(leadsView);
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

            var result = leadAdapter.GetLeadsByFilter(
                model.Field,
                model.SearchValue,
                model.OrderField,
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
        public ActionResult Edit(LeadViewModel lead, List<string> note, List<PhoneViewModel> newPhones)
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

                        var phones = leadToEdit.Phones;

                        for (int i = 0; i < phones.Count; i++)
                        {
                            var currentPhone = lead.Phones.FirstOrDefault(p => p.Id == phones[i].Id);
                            if (currentPhone != null)
                            {
                                phones[i].PhoneNumber = currentPhone.PhoneNumber;
                                phones[i].TypeId = (int?)currentPhone.Type;
                            }
                        }

                        if (newPhones != null)
                        {
                            var newLeadPhones = Mapper.Map<List<PhoneViewModel>, List<Phone>>(newPhones);
                            foreach (var item in newLeadPhones)
                            {
                                if (!string.IsNullOrWhiteSpace(item.PhoneNumber))
                                    leadToEdit.Phones.Add(item);
                            }
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
                        var notesToAdd = note
                            .Where(e => !string.IsNullOrWhiteSpace(e))
                            .Select(e => new Note() { Text = e, LeadId = lead.Id });

                        context.Notes.AddRange(notesToAdd);
                    }

                    context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ConvertLead(int id)
        {
            var customer = new UserViewModel();
            customer.Addresses.Add(new AddressViewModel());
            customer.Notes.Add(new NoteViewModel());

            using (var context = new BaseContext())
            {
                var currentLead = context.Leads
                    .Include(e => e.Phones)
                    .FirstOrDefault(e => e.Id == id);

                customer.Phones = Mapper.Map<List<PhoneViewModel>>(currentLead.Phones);

                var splitResult = currentLead.Name.Split(' ');

                if (splitResult.Length > 0)
                {
                    customer.FirstName = splitResult[0];
                    for (int i = 1; i < splitResult.Length; i++)
                    {
                        customer.LastName += $"{splitResult[i]} ";
                    }
                    customer.LastName = customer.LastName.TrimEnd();
                }
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
            var userCreads = User.GetCurrentUserCreads();

            LeadConvertService.Convert(model, newAddress, newPhones, userCreads.Email);

            return RedirectToAction("Index", "Customer");
        }
    }
}