using AutoMapper;
using CRM.Models;
using CRM.Services;
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
            List<CustomerViewModel> customers;
            using (BaseContext context = new BaseContext())
            {
                customers = Mapper.Map<List<Customer>, List<CustomerViewModel>>(context.Customers.Where(c => c.Id > 0).ToList());
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

            var items = Mapper.Map<List<Customer>, List<CustomerViewModel>>(SearchService<Customer>.Search(model));
            return PartialView("_CustomersPagePartial", items);
        }

        [HttpGet]
        public ActionResult Edit(string email)
        {
            CustomerViewModel customer;
            List<string> notes;
            using (BaseContext context = new BaseContext())
            {
                Customer customerDb = context.Customers.Include("Phones").FirstOrDefault(c => c.Email == email);
                customer = Mapper.Map<Customer, CustomerViewModel>(customerDb);
                notes = context.Notes.Where(n => n.LeadId == customerDb.Id).Select(i => i.Text).ToList();
                customer.Notes = notes;
            }
            if (customer != null)
            {
                return View(customer);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(CustomerViewModel customer, string oldEmail)
        {
            if (ModelState.IsValid)
            {
                using (BaseContext context = new BaseContext())
                {
                    Customer customerToEdit = context.Customers.FirstOrDefault(c => c.Email == oldEmail);
                    if (customerToEdit != null)
                    {
                        customerToEdit.Email = customer.Email;
                        customerToEdit.FirstName = customer.FirstName;
                        //customerToEdit.Phones = customer.Phone;
                    }
                    if (customer.Notes.Any())
                    {
                        foreach (string note in customer.Notes)
                        {
                            context.Notes.Add(new Note { LeadId = customerToEdit.Id, Text = note });
                        }
                    }
                    context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SendMessage(string oldEmail, string message)
        {
            var leadEmail = "";
            var currentUserEmail = User.Identity.Name.Split('|')[1];
            using (BaseContext context = new BaseContext())
            {
                Lead lead = context.Leads.FirstOrDefault(l => l.Email == oldEmail);
                if (lead != null)
                {
                    leadEmail = lead?.Email;
                    if (lead.LeadOwner == 0)
                    {
                        var currentUser = context.Users.FirstOrDefault(u => u.Email == currentUserEmail);
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
    }
}