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
using System.Linq.Expressions;

namespace CRM.Controllers
{
    [Authenticate]
    public class LeadController : Controller
    {
        public ActionResult Index()
        {
            var columns = ReflectionService.GetModelProperties<LeadViewModel>();
            SearchViewModel model = new SearchViewModel();

            var leadAdapter = new LeadAdapter();
            int totalItems;

            var result = leadAdapter.GetLeadsByFilter(
                model.Field,
                model.SearchValue,
                model.OrderField,
                model.Page,
                model.ItemsPerPage,
                out totalItems,
                model.OrderDirection);

            var items = Mapper.Map<List<Lead>, List<LeadViewModel>>(result);
            model.Items = items;
            model.ItemsCount = totalItems;
            foreach (var i in columns)
            {
                model.Columns.Add(new GridFieldViewModel(i, true, 0));
            }
            return View(model);
        }

        private IEnumerable<Lead> ListStores(Expression<Func<Lead, string>> sort, bool desc, int page, int pageSize, out int totalRecords)
        {
            List<Lead> leads = new List<Lead>();
            using (var context = new BaseContext())
            {
                totalRecords = context.Leads.Count();
                int skipRows = (page - 1) * pageSize;
                if (desc)
                    leads = context.Leads.Include(l => l.Phones).Include(l => l.Notes).OrderByDescending(sort).Skip(skipRows).Take(pageSize).ToList();
                else
                    leads = context.Leads.Include(l => l.Phones).Include(l => l.Notes).OrderBy(sort).Skip(skipRows).Take(pageSize).ToList();
            }
            return leads;
        }

        [HttpPost]
        public ActionResult Search(SearchViewModel model)
        {
            model.TableName = "Leads";

            var leadAdapter = new LeadAdapter();
            int totalItems;

            var result = leadAdapter.GetLeadsByFilter(
                model.Field,
                model.SearchValue,
                model.OrderField,
                model.Page,
                model.ItemsPerPage,
                out totalItems,
                model.OrderDirection);

            var items = Mapper.Map<List<Lead>, List<LeadViewModel>>(result);
            model.Items = items;
            model.ItemsCount = totalItems;
            return PartialView("_LeadsPagePartial", model);
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
                            var currentPhone = lead.Phones.FirstOrDefault(p=>p.Id == phones[i].Id);
                            if (currentPhone != null)
                            {
                                phones[i].PhoneNumber = currentPhone.PhoneNumber;
                                phones[i].TypeId = (int?)currentPhone.Type;
                            }
                        }

                        if (newPhones != null)
                        {
                            var newLeadPhones = Mapper.Map<List<PhoneViewModel>, List<Phone>>(newPhones);
                            foreach(var item in newLeadPhones)
                            {
                                if(!string.IsNullOrWhiteSpace(item.PhoneNumber))
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