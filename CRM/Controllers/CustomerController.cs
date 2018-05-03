using AutoMapper;
using CRM.Enums;
using CRM.Extentions;
using CRM.Models;
using CRM.Services;
using CRMData.Adapters;
using CRMData.Contexts;
using CRMData.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    public class CustomerController : Controller
    {
        public ActionResult Index()
        {
            List<UserViewModel> customers;
            using (BaseContext context = ContextFactory.SingleContextFactory.Get<BaseContext>())
            {
                customers = Mapper.Map<List<User>, List<UserViewModel>>(context.Users
                    .Where(e => e.UserTypeId == (int)UserType.Customer)
                    .Include(e => e.Phones)
                    .ToList());
            }
            return View(customers);
        }

        [HttpPost]
        public ActionResult Search(SearchViewModel model)
        {
            model.TableName = "Customers";

            if (!ModelState.IsValid)
            {
                return Json(new { status = "error", message = "Model is not valid!" });
            }

            var customerAdapter = new UserAdapter();

            var result = customerAdapter.GetUsersByFilter(
                model.Field,
                model.SearchValue,
                model.OrderField,
                (int)UserType.Customer,
                model.OrderDirection.Equals("ASC"));

            var items = Mapper.Map<List<User>, List<UserViewModel>>(result);
            return PartialView("_CustomersPagePartial", items);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            UserViewModel customer;
            using (BaseContext context = ContextFactory.SingleContextFactory.Get<BaseContext>())
            {
                ; User customerDb = context.Users
                      .Include(e => e.Phones)
                      .Include(e => e.Addresses)
                      //.Include(e => e.Lead)
                      .FirstOrDefault(c => c.Id == id);

                customer = Mapper.Map<User, UserViewModel>(customerDb);

                //List<Note> customerNote = context.Notes.Where(n => n.LeadId == customerDb.Lead.Id).ToList();
                List<Note> customerNote = context.Notes.Where(n => n.UserId == customerDb.Id).ToList();
                customer.Notes = Mapper.Map<List<Note>, List<NoteViewModel>>(customerNote);

                customer.Addresses = Mapper.Map<List<Address>, List<AddressViewModel>>((List<Address>)customerDb.Addresses);

                if (customer.Addresses.Count == 0)
                {
                    customer.Addresses.Add(new AddressViewModel());
                }
            }
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
                using (BaseContext context = ContextFactory.SingleContextFactory.Get<BaseContext>())
                {
                    User customerToEdit = context.Users
                        .Include(e => e.Phones)
                        .Include(e => e.Addresses)
                        .FirstOrDefault(c => c.Id == user.Id);

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
                            //else
                            //{
                            //    customerToEdit.Phones.Add(incomePhone);
                            //}
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
                            Note oldNote = context.Notes.FirstOrDefault(e => e.Id == reNewNote.Id);
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

                        context.Notes.AddRange(notesToAdd);
                    }

                    // Додавання нових телефонів
                    if (newPhones != null && newPhones.Any())
                    {
                        var newUserPhones = Mapper.Map<List<Phone>>(newPhones)
                            .Where(e => !string.IsNullOrWhiteSpace(e.PhoneNumber))
                            .ToList();

                        newUserPhones.ForEach(phone => phone.UserId = user.Id);

                        context.Phones.AddRange(newUserPhones);
                    }
                    // Додавання нових адрес
                    if (newAddress != null && newAddress.Any())
                    {
                        var newUserAddresses = Mapper.Map<List<Address>>(newAddress)
                            .Where(e => !(string.IsNullOrWhiteSpace(e.Line1) || string.IsNullOrWhiteSpace(e.Line2)))
                            .ToList();

                        newUserAddresses.ForEach(address => address.UserId = user.Id);

                        context.Addresses.AddRange(newUserAddresses);
                    }
                    context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}