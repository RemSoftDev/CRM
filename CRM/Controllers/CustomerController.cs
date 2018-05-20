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
using CRM.DAL.Contexts;
using System.Data.Entity;

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

            return View(GetSearchModel(currentUserEmail));
        }
        private SearchViewModel FillModelByProfile(GridProfileViewModel profile)
        {
            var userAdapter = new UserAdapter();
            var model = new SearchViewModel();

            model.Columns = profile?.GridFields ?? new List<GridFieldViewModel>();
            model.Field = profile?.SearchField;
            model.SearchValue = profile?.SearchValue;

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

            if (!model.Columns.Any())
            {
                var columns = ReflectionService.GetModelProperties<UserViewModel>();
                for (int i = 0; i < columns.Count; i++)
                {
                    model.Columns.Add(new GridFieldViewModel(columns[i], true, 0, i));
                }
            }

            return model;
        }

        private SearchViewModel GetSearchModel(string userEmail)
        {
            var userAdapter = new UserAdapter();
            var model = new SearchViewModel();
            var profiles = Mapper.Map<List<GridProfileViewModel>>(
                userAdapter.GetUserProfiles(userEmail)
            );

            var selectedProfile = profiles.FirstOrDefault(p => p.IsDefault);
            model = FillModelByProfile(selectedProfile);
            model.Profiles = profiles;

            return model;
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

            return PartialView("_GridPagePartial", model);
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

            var userAdapter = new UserAdapter();

            var profiles = Mapper.Map<List<GridProfileViewModel>>(
                userAdapter.GetUserProfiles(currentUserEmail, profileName)
            );

            return PartialView("_GridPagePartial", FillModelByProfile(profiles.FirstOrDefault()));
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
                            .Equals("User"))
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
                                g.DGrid.Type.Equals("User"))
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
                            .Equals("User"));

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
                    .FirstOrDefault(g => g.DGrid.Type.Equals("User"))
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
                        context.DGrids.Add(new DGrid { Type = "User", GridProfiles = new List<GridProfile> { profile } });
                    }

                    context.SaveChanges();

                    return Json(new { success = true });
                }
            }

            return Json(new { success = false });
        }
    }
}