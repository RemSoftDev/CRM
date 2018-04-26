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
        public static void Convert(
            LeadConvertViewModel model,
            List<AddressViewModel> newAddress,
            List<PhoneViewModel> newPhones,
            string currentUserEmail)
        {
            using (BaseContext context = ContextFactory.SingleContextFactory.Get<BaseContext>())
            {
                var lead = context.Leads.Include(e => e.Phones).FirstOrDefault(l => l.Id == model.Id && l.Customer == null);

                var newCustomer = Mapper.Map<CustomerViewModel, Customer>(model.NewCustomer);
#warning после мапа и сохраниени изменений при пустом Notes сохраняет пустую запись. Также дублирует существующие телефоны лида в базе

                newCustomer.Email = lead.Email;
                newCustomer.Lead = lead;

                if (newPhones != null && newPhones.Count != 0)
                {
                    var newPhoneAfterMap = Mapper.Map<List<PhoneViewModel>, List<Phone>>(newPhones);

                    for (int i = 0; i < newPhoneAfterMap.Count; i++)
                    {
                        newCustomer.Phones.Add(newPhoneAfterMap[i]);
                    }
                }

                if (newAddress != null && newAddress.Count != 0)
                {
                    var newAddressAfterMap = Mapper.Map<List<AddressViewModel>, List<Address>>(newAddress);

                    for (int i = 0; i < newAddressAfterMap.Count; i++)
                    {
                        newCustomer.Addresses.Add(newAddressAfterMap[i]);
                    }
                }

#warning НУЖНО ДОПИСАТЬ РАБОТУ С НОВЫМИ Notes
                var newCustomerInDB = context.Customers.Add(newCustomer);

                // логирование процесса конвертации
                context.LeadConvertedLogs.Add(new LeadConvertedLog()
                {
                    LeadId = model.Id,
                    CustomerId = newCustomerInDB.Id,
                    ConvertDateTime = DateTime.Now,
                    ConvertedByUserId = context.Users.FirstOrDefault(e => e.Email.Equals(currentUserEmail)).Id
                });

                context.SaveChanges();
                             
            }
        }
    }
}