using AutoMapper;
using CRM.Attributes;
using CRM.DAL.Adapters;
using CRM.DAL.Entities;
using CRM.DAL.Repository;
using CRM.Extentions;
using CRM.Hubs;
using CRM.Managers;
using CRM.Models;
using CRM.Services;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CRM.Controllers
{
    [Authenticate]
    public class LeadController : BaseController
    {
        //private readonly IUnitOfWork _unitOfWork;
        private readonly ILeadConvertService _leadConvertService;
        private readonly ILeadManager _leadManager;
        private readonly IEmailService _emailService;

        public LeadController(
            IUnitOfWork unitOfWork,
            ILeadConvertService leadConvertService,
            ILeadManager leadManager,
            IEmailService emailService)
            : base(unitOfWork)
        {
            //_unitOfWork = unitOfWork;
            _leadConvertService = leadConvertService;
            _leadManager = leadManager;
            _emailService = emailService;

        }
        public ActionResult Index()
        {
            var currentUserEmail = User.GetCurrentUserCreads()?.Email;
            var service = new SearchService<LeadViewModel>();

            return View(service.GetSearchModel(currentUserEmail));
        }

        [HttpPost]
        public ActionResult Search(SearchViewModel model)
        {
            model.TableName = "Leads";
            var currentUserEmail = User.GetCurrentUserCreads()?.Email;

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
            model.Items = items.ToList<Interfaces.IUser>();
            model.ItemsCount = totalItems;

            return PartialView("~/Views/Grid/_GridPagePartial.cshtml", model);
        }

        [HttpPost]
        public ActionResult LoadProfile(string profileName)
        {
            var currentUserEmail = User.GetCurrentUserCreads().Email;

            var leadAdapter = new LeadAdapter();
            var service = new SearchService<LeadViewModel>();

            var profiles = Mapper.Map<List<GridProfileViewModel>>(
                service.GetUserProfiles(currentUserEmail, profileName)
            );

            return PartialView("~/Views/Grid/_GridPagePartial.cshtml", service.FillLeadModelByProfile(profiles.FirstOrDefault()));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateLeadViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Phones = new List<PhoneViewModel>
                {
                    new PhoneViewModel
                    {
                        PhoneNumber = model.Phone,
                        Type = Enums.PhoneType.HomePhone
                    }
                };

                // Set IsSaved = true, its mean that the lead comes not from POST request
                var lead = Mapper.Map<CreateLeadViewModel, Lead>(model);
                lead.IsSaved = true;

                var result = _leadManager.Create(lead);

                if (!string.IsNullOrEmpty(model.Message))
                {
                    EmailViewModel email = new EmailViewModel
                    {
                        To = "csharpcrm@gmail.com",
                        Subject = "Lead Registered",
                        Body = model.Message,
                        WasReceived = true
                    };

                    model.Id = _leadManager.GetLead(model.Email).Id;
                    new Task(() => { _emailService.SendEmail(email, model); }).Start();
                }

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index), "Lead");
                }

                AddErrors(result);
            }

            return View(model);
        }

        private void AddErrors(IdentityResult result)
        {
            //_unitOfWork.Dispose(!disposing);
            //base.Dispose(disposing);
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


        public ActionResult Edit(int id)
        {
            var leadModel = _unitOfWork.LeadsRepository.FindById(id);
            var leadNoteModel = _unitOfWork.NotesRepository.Get(n => n.LeadId == leadModel.Id).ToList();

            var leadViewModel = Mapper.Map<Lead, LeadViewModel>(leadModel);
            leadViewModel.Notes = Mapper.Map<List<Note>, List<NoteViewModel>>(leadNoteModel);
            if (leadViewModel != null)
            {
                return View(leadViewModel);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(LeadViewModel lead, List<string> note, List<PhoneViewModel> newPhones)
        {
            if (ModelState.IsValid)
            {
                var leadToEdit = _unitOfWork.LeadsRepository.FindById(lead.Id);

                if (leadToEdit != null)
                {
                    leadToEdit.Email = lead.Email;
                    leadToEdit.Name = lead.Name;
                    leadToEdit.Discipline = lead.Discipline;
                    leadToEdit.AgeGroup = lead.AgeGroup;
                    leadToEdit.Status = lead.Status;
                    leadToEdit.StatusNotes = lead.StatusNotes;
                    leadToEdit.IssueRaised = lead.IssueRaised;
                    leadToEdit.IsSaved = true;

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
                        var oldNote = _unitOfWork.NotesRepository.FindById(reNewNote.Id);

                        if (oldNote != null)
                            oldNote.Text = reNewNote.Text;
                    }
                }

                if (note != null && note.Any())
                {
                    var notesToAdd = note
                        .Where(e => !string.IsNullOrWhiteSpace(e))
                        .Select(e => new Note() { Text = e, LeadId = lead.Id });

                    _unitOfWork.NotesRepository.AddRange(notesToAdd);
                }

                _unitOfWork.Save();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ConvertLead(int id)
        {
            var customer = new UserViewModel();
            customer.Addresses.Add(new AddressViewModel());
            customer.Notes.Add(new NoteViewModel());

            var currentLead = _unitOfWork.LeadsRepository.FindById(id);

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

            _leadConvertService.Convert(model, newAddress, newPhones, userCreads.Email);

            return RedirectToAction("Index", "Customer");
        }

        [HttpPost]
        public ActionResult EditProfile(SearchViewModel model, bool makeDefault)
        {
            var currentUserEmail = User.GetCurrentUserCreads().Email;
            var service = new SearchService<LeadViewModel>();

            var editProfileResult = service.EditProfile(model, makeDefault, currentUserEmail);

            return Json(new { success = editProfileResult });
        }

        [HttpPost]
        public JsonResult CreateProfile(SearchViewModel model, string profileName)
        {
            if (!string.IsNullOrWhiteSpace(profileName))
            {
                var currentUserEmail = User.GetCurrentUserCreads().Email;
                var service = new SearchService<LeadViewModel>();
                var createProfileResult = service.CreateProfile(model, profileName, currentUserEmail);

                return Json(new { success = createProfileResult, message = "Profile with this name already exists!" });
            }

            return Json(new { success = false, message = "Name can't be empty!" });
        }

        [HttpPost]
        public ActionResult Notes(int id)
        {
            var service = new SearchService<LeadViewModel>();
            var notes = service.GetLeadNotes(id);

            return PartialView("~/Views/Grid/_Notes.cshtml", notes);
        }
    }
}