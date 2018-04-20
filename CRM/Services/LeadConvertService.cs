using AutoMapper;
using CRM.Models;
using CRMData.Contexts;
using CRMData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Services
{
    public static class LeadConvertService
    {
        public static void Convert(CustomerViewModel model, int leadId, string currentUserEmail)
        {
            using (BaseContext context = ContextFactory.SingleContextFactory.Get<BaseContext>())
            {
                var lead = context.Leads.Include("Phones").FirstOrDefault(l => l.Id == leadId && l.Customer == null);

                if(lead != null)
                {
                    lead.LeadOwner = context.Users.FirstOrDefault(u => u.Email == currentUserEmail).Id;
                    var customer = Mapper.Map<CustomerViewModel, Customer>(model);

                    customer.Phones = lead.Phones;
                    customer.Email = lead.Email;
                    customer.Lead = lead;

                    context.Customers.Add(customer);
                    context.SaveChanges();
                }                
            }
        }
    }
}