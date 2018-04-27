using AutoMapper;
using CRM.Models;
using CRMData.Contexts;
using CRMData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using CRM.Enums;

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
                var lead = context.Leads
                    .Include(e => e.Phones)
                    .FirstOrDefault(l => l.Id == model.Id && l.User == null);
                
                // Мапим нашего нового юзера(Телефоны обнуляем, так как запишет дубыль)
                var newCustomer = Mapper.Map<UserViewModel, User>(model.NewCustomer);
                newCustomer.UserTypeId = (int)UserType.Customer;
                newCustomer.Phones = new List<Phone>();
                newCustomer.Email = lead.Email;

                // Добавляем новые телефоны
                if (newPhones != null && newPhones.Count != 0)
                {
                    var newPhoneAfterMap = Mapper.Map<List<PhoneViewModel>, List<Phone>>(newPhones);

                    foreach(var phone in newPhoneAfterMap)
                    {
                        newCustomer.Phones.Add(phone);
                    }
                }

                // Добавляем новые адресса
                if (newAddress != null && newAddress.Count != 0)
                {
                    var newAddressAfterMap = Mapper.Map<List<AddressViewModel>, List<Address>>(newAddress);

                    foreach(var address in newAddressAfterMap)
                    {
                        newCustomer.Addresses.Add(address);
                    }
                }

                // Для конвертации не нужно
                //if(newCustomer.Notes.Count > 0)
                //{
                //    for (int i = 0; i < newCustomer.Notes.Count; i++)
                //    {
                //        if(string.IsNullOrEmpty(newCustomer.Notes[i].Text) && string.IsNullOrWhiteSpace(newCustomer.Notes[i].Text))
                //            newCustomer.Notes.RemoveAt(i);
                //    }
                //}

                var newCustomerInDB = context.Users.Add(newCustomer);

                // Поле того как получили айди новосозданного юзера, прописываем его в телефон который был прикреплен к лиду
                var leadPhones = context.Phones.Where(e => e.LeadId == model.Id).ToList();
                foreach (var phone in model.NewCustomer.Phones)
                {
                    var leadPhone = leadPhones.FirstOrDefault(e => e.Id == phone.Id);
                    leadPhone = Mapper.Map<PhoneViewModel, Phone>(phone, leadPhone);
                    leadPhone.UserId = newCustomerInDB.Id;
                }

                // прикрипляем записи лида к ново созданному юзеру
                context.Notes
                    .Where(e => e.LeadId == model.Id)
                    .ForEachAsync(e => e.UserId = newCustomerInDB.Id)
                    .Wait();

                // проставляем что лида перевели в кастомера
                lead.IsConverted = true;

                // логирование процесса конвертации
                context.LeadConvertedLogs.Add(new LeadConvertedLog()
                {
                    LeadId = model.Id,
                    UserId = newCustomerInDB.Id,
                    ConvertDateTime = DateTime.Now,
                    ConvertedByUserId = context.Users.FirstOrDefault(e => e.Email.Equals(currentUserEmail)).Id
                });

                context.SaveChanges();
            }
        }
    }
}