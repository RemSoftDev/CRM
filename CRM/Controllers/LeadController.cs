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
            var currentUserEmail = User.GetCurrentUserCreads().Email;

            return View(GetSearchModel(currentUserEmail));
        }

        private SearchViewModel GetSearchModel(string userEmail)
        {
            var leadAdapter = new LeadAdapter();
            var model = new SearchViewModel();
            var profiles = Mapper.Map<List<GridProfileViewModel>>(
                leadAdapter.GetUserProfiles(userEmail)
            );

            var selectedProfile = profiles.FirstOrDefault(p => p.IsDefault) ?? profiles.First();
            model = FillModelByProfile(selectedProfile);
            model.Profiles = profiles;

            return model;
        }

        private SearchViewModel FillModelByProfile(GridProfileViewModel profile)
        {
            var leadAdapter = new LeadAdapter();
            var model = new SearchViewModel();

            model.Columns = profile.GridFields;
            model.Field = profile.SearchField;
            model.SearchValue = profile.SearchValue;

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

            if (!model.Columns.Any())
            {
                var columns = ReflectionService.GetModelProperties<LeadViewModel>();
                for (int i = 0; i < columns.Count; i++)
                {
                    model.Columns.Add(new GridFieldViewModel(columns[i], true, 0, i));
                }
            }

            return model;
        }

        [HttpPost]
        public ActionResult Search(SearchViewModel model)
        {
            model.TableName = "Leads";
            var currentUserEmail = User.GetCurrentUserCreads().Email;

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

        [HttpPost]
        public ActionResult LoadProfile(string profileName)
        {
            var currentUserEmail = User.GetCurrentUserCreads().Email;

            var leadAdapter = new LeadAdapter();

            var profiles = Mapper.Map<List<GridProfileViewModel>>(
                leadAdapter.GetUserProfiles(currentUserEmail, profileName)
            );

            return PartialView("_LeadsPagePartial", FillModelByProfile(profiles.FirstOrDefault()));
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

        [HttpPost]
        public ActionResult EditProfile(SearchViewModel model, bool makeDefault)
        {
            var currentUserEmail = User.GetCurrentUserCreads().Email;

            using (var context = new BaseContext())
            {
                var userId = context.Users
                    .Include(u => u.GridProfiles.Select(g => g.DGrid))
                    .FirstOrDefault(u => u.Email
                        .Equals(currentUserEmail)).Id;

                var selectedProfile = model.Profiles.FirstOrDefault(p => p.IsDefault);

                var profile = context
                    .GridProfiles
                    .Include(f => f.GridFields)
                    .Where(g =>
                        g.ProfileName
                            .Equals(selectedProfile.ProfileName) &&
                        g.UserId
                            .Equals(userId) &&
                        g.DGrid.Type
                            .Equals("Lead"))
                    .FirstOrDefault();

                if (profile != null)
                {
                    var receivedFields = Mapper.Map<List<GridField>>(model.Columns);
                    var fields = profile.GridFields;

                    for (int i = 0; i < fields.Count; i++)
                    {
                        fields[i].ColumnName = receivedFields[i].ColumnName;
                        fields[i].GridOrderDirection = receivedFields[i].GridOrderDirection;
                        fields[i].IsActive = receivedFields[i].IsActive;
                        fields[i].Order = receivedFields[i].Order;
                    }

                    if (makeDefault)
                    {
                        context
                            .GridProfiles
                            .Where(g =>
                                g.UserId.Equals(userId) &&
                                g.DGrid.Type.Equals("Lead"))
                            .ForEachAsync(i => i.IsDefault = false)
                            .Wait();

                        profile.IsDefault = makeDefault;
                    }

                    profile.SearchField = model.Field;
                    profile.SearchValue = model.SearchValue;

                    context.SaveChanges();

                    return Json(new { success = true });
                }
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public JsonResult CreateProfile(SearchViewModel model, string profileName)
        {
            var currentUserEmail = User.GetCurrentUserCreads().Email;
            using (var context = new BaseContext())
            {
                var user = context.Users
                    .Include(u => u.GridProfiles.Select(g => g.DGrid))
                    .FirstOrDefault(u => u.Email
                        .Equals(currentUserEmail));

                var profiles = context
                    .GridProfiles
                    .Where(g =>
                        g.ProfileName
                            .Equals(profileName) &&
                        g.UserId
                            .Equals(user.Id) &&
                        g.DGrid.Type
                            .Equals("Lead"));

                if (!profiles.Any() && user.Id > 0)
                {
                    var receivedFields = Mapper.Map<List<GridField>>(model.Columns);
                    var profile = new GridProfile
                    {
                        GridFields = receivedFields,
                        IsDefault = true,
                        SearchField = model.Field,
                        SearchValue = model.SearchValue,
                        ProfileName = profileName,
                        UserId = user.Id
                    };

                    var leadsGrid = user
                    .GridProfiles
                    .FirstOrDefault(g => g.DGrid.Type.Equals("Lead"))
                    ?.DGrid;

                    if (leadsGrid != null)
                    {
                        foreach (var item in leadsGrid.GridProfiles)
                        {
                            item.IsDefault = false;
                        }

                        leadsGrid.GridProfiles.Add(profile);
                    }
                    else
                    {
                        context.DGrids.Add(new DGrid { Type = "Lead", GridProfiles = new List<GridProfile> { profile } });
                    }

                    context.SaveChanges();

                    return Json(new { success = true });
                }
            }

            return Json(new { success = false });
        }
    }
}