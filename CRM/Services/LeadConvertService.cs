using AutoMapper;
using CRM.Models;
using CRMData.Contexts;
using CRMData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace CRM.Services
{
    public static class LeadConvertService
    {
        public static void Convert(LeadConvertViewModel model, string currentUserEmail)
        {
            using (BaseContext context = ContextFactory.SingleContextFactory.Get<BaseContext>())
            {
                var lead = context.Leads.Include(e => e.Phones).FirstOrDefault(l => l.Id == model.Id && l.Customer == null);

                if(lead != null)
                {
                    lead.LeadOwner = context.Users.FirstOrDefault(u => u.Email == currentUserEmail)?.Id;
                    var customer = Mapper.Map<CustomerViewModel, Customer>(model.NewCustomer);

                    customer.Phones = lead.Phones;
                    customer.Email = lead.Email;
                    customer.Lead = lead;

                    var newCustomer = context.Customers.Add(customer);

                    // логирование процесса конвертации
                    context.LeadConvertedLogs.Add(new LeadConvertedLog()
                    {
                        LeadId = model.Id,
                        CustomerId = newCustomer.Id,
                        ConvertDateTime = DateTime.Now,
                        ConvertedByUserId = context.Users.FirstOrDefault(e => e.Email.Equals(currentUserEmail)).Id
                    });

                    context.SaveChanges();
                }                
            }
        }
    }
}