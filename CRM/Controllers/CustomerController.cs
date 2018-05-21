using AutoMapper;
using CRM.DAL.Adapters;
using CRM.DAL.Entities;
using CRM.DAL.Repository;
using CRM.Extentions;
using CRM.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CRM.Interfaces;
using CRM.Services;

namespace CRM.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork.ValidateNotDefault(nameof(unitOfWork));
        }

        public ActionResult Index()
        {
            var currentUserEmail = User.GetCurrentUserCreads().Email;
            var service = new SearchService<UserViewModel>();

            return View(service.GetSearchModel(currentUserEmail));
        }

        [HttpPost]
        public ActionResult Search(SearchViewModel model)
        {
            model.TableName = "Users";
            var currentUserEmail = User.GetCurrentUserCreads().Email;

            var userAdapter = new UserAdapter();
            int totalItems;

            var result = userAdapter.GetUsersByFilter(
                model.Field,
                model.SearchValue,
                model.OrderField,
                model.Page,
                model.ItemsPerPage,
                out totalItems,
                model.OrderDirection);

            var items = Mapper.Map<List<User>, List<UserViewModel>>(result);
            model.Items = items.ToList<IUser>();
            model.ItemsCount = totalItems;

            return PartialView("~/Views/Grid/_GridPagePartial.cshtml", model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var cumstomerModel = _unitOfWork.UsersRepository.FindById(id);
            var customer = Mapper.Map<User, UserViewModel>(cumstomerModel);

            if (customer != null)
            {
                return View(customer);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(UserViewModel user, List<string> note, List<AddressViewModel> newAddress, List<PhoneViewModel> newPhones)
        {
            if (ModelState.IsValid)
            {
	            var customerToEdit = _unitOfWork.UsersRepository.FindById(user.Id);


				if (customerToEdit != null)
                {
                    customerToEdit.Title = user.Title;
                    customerToEdit.Email = user.Email;
                    customerToEdit.FirstName = user.FirstName;
                    customerToEdit.LastName = user.LastName;

                    // Изменение телефонов
                    var phones = Mapper.Map<List<Phone>>(user.Phones);
                    phones.ForEach(incomePhone =>
                    {
                        var phoneToEdit = customerToEdit.Phones.FirstOrDefault(c => c.Id == incomePhone.Id);
                        if (phoneToEdit != null)
                        {
                            phoneToEdit.PhoneNumber = incomePhone.PhoneNumber;
                            phoneToEdit.TypeId = incomePhone.TypeId;
                        }

                    });

                    // Изменение адресов
                    var addresses = Mapper.Map<List<Address>>(user.Addresses);
                    addresses.ForEach(incomeAddress =>
                    {
                        var addrToEdit = customerToEdit.Addresses.FirstOrDefault(c => c.Id == incomeAddress.Id);
                        if (addrToEdit != null)
                        {
                            addrToEdit.Line1 = incomeAddress.Line1;
                            addrToEdit.Line2 = incomeAddress.Line2;
                            addrToEdit.PostCode = incomeAddress.PostCode;
                            addrToEdit.Town = incomeAddress.Town;
                            addrToEdit.Country = incomeAddress.Country;
                            addrToEdit.County = incomeAddress.County;
                            addrToEdit.AddressTypeId = incomeAddress.AddressTypeId;
                            addrToEdit.AddressType = incomeAddress.AddressType;
                        }
                        else
                        {
                            addresses.Where(e => !(string.IsNullOrWhiteSpace(e.Line1) || string.IsNullOrWhiteSpace(e.Line2)))
                            .ToList()
                            .ForEach(e => customerToEdit.Addresses.Add(e));
                        }
                    });
                }

                // Изменение записей
                if (user.Notes.Any())
                {
                    foreach (NoteViewModel reNewNote in user.Notes)
                    {
	                    var oldNote = _unitOfWork.NotesRepository.FindById(reNewNote.Id);

						if (oldNote != null)
                            oldNote.Text = reNewNote.Text;
                    }
                }

                // Добавление новых записей
                if (note != null && note.Any())
                {
                    var notesToAdd = note
                        .Where(e => !string.IsNullOrWhiteSpace(e))
                        .Select(e => new Note() { Text = e, UserId = user.Id });

					_unitOfWork.NotesRepository.AddRange(notesToAdd);
                }

                // Додавання нових телефонів
                if (newPhones != null && newPhones.Any())
                {
                    var newUserPhones = Mapper.Map<List<Phone>>(newPhones)
                        .Where(e => !string.IsNullOrWhiteSpace(e.PhoneNumber))
                        .ToList();

                    newUserPhones.ForEach(phone => phone.UserId = user.Id);

					_unitOfWork.PhonesRepository.AddRange(newUserPhones);
                }
                // Додавання нових адрес
                if (newAddress != null && newAddress.Any())
                {
                    var newUserAddresses = Mapper.Map<List<Address>>(newAddress)
                        .Where(e => !(string.IsNullOrWhiteSpace(e.Line1) || string.IsNullOrWhiteSpace(e.Line2)))
                        .ToList();

                    newUserAddresses.ForEach(address => address.UserId = user.Id);

					_unitOfWork.AddressRepository.AddRange(newUserAddresses);
                }
	            _unitOfWork.Save();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult LoadProfile(string profileName)
        {
            var currentUserEmail = User.GetCurrentUserCreads().Email;
            var service = new SearchService<UserViewModel>();

            var profiles = Mapper.Map<List<GridProfileViewModel>>(
                service.GetUserProfiles(currentUserEmail, profileName)
            );

            return PartialView("~/Views/Grid/_GridPagePartial.cshtml", service.FillUserModelByProfile(profiles.FirstOrDefault()));
        }

        [HttpPost]
        public ActionResult EditProfile(SearchViewModel model, bool makeDefault)
        {
            var currentUserEmail = User.GetCurrentUserCreads().Email;
            var service = new SearchService<UserViewModel>();

            var editProfileResult = service.EditProfile(model, makeDefault, currentUserEmail);

            return Json(new { success = editProfileResult });
        }

        [HttpPost]
        public JsonResult CreateProfile(SearchViewModel model, string profileName)
        {
            if (!string.IsNullOrWhiteSpace(profileName))
            {
                var currentUserEmail = User.GetCurrentUserCreads().Email;
                var service = new SearchService<UserViewModel>();
                var createProfileResult = service.CreateProfile(model, profileName, currentUserEmail);

                return Json(new { success = createProfileResult, message = "Profile with this name already exists!" });
            }

            return Json(new { success = false, message = "Name can't be empty!" });
        }

        [HttpPost]
        public ActionResult Notes(int id)
        {
            var service = new SearchService<UserViewModel>();
            var notes = service.GetUserNotes(id);

            return PartialView("~/Views/Grid/_Notes.cshtml", notes);
        }
    }
}