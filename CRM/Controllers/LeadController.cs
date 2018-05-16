using AutoMapper;
using CRM.Attributes;
using CRM.Extentions;
using CRM.Models;
using CRM.Services;
using CRMData.Adapters;
using CRMData.Contexts;
using CRMData.Entities;
using CRMData.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CRM.Controllers
{
    [Authenticate]
    public class LeadController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public LeadController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Index()
        {
            var leadsNotConverted = _unitOfWork.LeadsRepository.GetWithInclude(l => !l.IsConverted);
            var leadsView = Mapper.Map<IEnumerable<Lead>, IEnumerable<LeadViewModel>>(leadsNotConverted); ;

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
            var leadModel = _unitOfWork.LeadsRepository.FindById(id);
            var leadNoteModel = _unitOfWork.NotesRepository.Get(n => n.LeadId == leadModel.Id);

            var leadViewModel = Mapper.Map<Lead, LeadViewModel>(leadModel);
            leadViewModel.Notes = Mapper.Map<IEnumerable<Note>, IEnumerable<NoteViewModel>>(leadNoteModel).ToList();

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

            LeadConvertService.Convert(model, newAddress, newPhones, userCreads.Email);

            return RedirectToAction("Index", "Customer");
        }

        protected override void Dispose(bool disposing)
        {
            //_unitOfWork.Dispose(!disposing);
            base.Dispose(disposing);
        }
    }
}