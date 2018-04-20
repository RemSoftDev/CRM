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
            using (BaseContext context = ContextFactory.SingleContextFactory.Get<BaseContext>())
            {
                customers = Mapper.Map<List<Customer>, List<CustomerViewModel>>(context
                    .Customers
                    .Include("Phones")
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

            var items = Mapper.Map<List<Customer>, List<CustomerViewModel>>(SearchService<Customer>.Search(model));
            return PartialView("_CustomersPagePartial", items);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            CustomerViewModel customer;
            List<string> notes;
            using (BaseContext context = ContextFactory.SingleContextFactory.Get<BaseContext>())
            {
;               Customer customerDb = context.Customers.Include("Phones").Include("Lead").FirstOrDefault(c => c.Id == id);
                customer = Mapper.Map<Customer, CustomerViewModel>(customerDb);
                notes = context.Notes.Where(n => n.LeadId == customerDb.Lead.Id).Select(i => i.Text).ToList();
                customer.Notes = notes;
            }
            if (customer != null)
            {
                return View(customer);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(CustomerViewModel customer, int id)
        {
            if (ModelState.IsValid)
            {
                using (BaseContext context = ContextFactory.SingleContextFactory.Get<BaseContext>())
                {
                    Customer customerToEdit = context.Customers.Include("Phones").FirstOrDefault(c => c.Id == id);
                    if (customerToEdit != null)
                    {
                        customerToEdit.Title = customer.Title;
                        customerToEdit.Email = customer.Email;
                        customerToEdit.FirstName = customer.FirstName;
                        customerToEdit.LastName = customer.LastName;
                        customerToEdit.Phones
                            .FirstOrDefault()
                            .PhoneNumber = customer.Phones.FirstOrDefault().PhoneNumber;
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
        public ActionResult SendMessage(int id, string message)
        {
            var leadEmail = "";
            var currentUserEmail = User.Identity.Name.Split('|')[1];
            using (BaseContext context = ContextFactory.SingleContextFactory.Get<BaseContext>())
            {
                Customer customer = context.Customers.FirstOrDefault(c => c.Id == id);
                if (customer != null)
                {
                    leadEmail = customer?.Email;
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