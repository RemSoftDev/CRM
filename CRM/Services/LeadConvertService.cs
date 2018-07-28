using AutoMapper;
using CRM.DAL.Entities;
using CRM.DAL.Repository;
using CRM.Enums;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WebGrease.Css.Extensions;

namespace CRM.Services
{
	public class LeadConvertService : ILeadConvertService
	{
		private readonly IUnitOfWork _unitOfWork;

		public LeadConvertService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public void Convert(
			LeadConvertViewModel model,
			List<AddressViewModel> newAddress,
			List<PhoneViewModel> newPhones,
			string currentUserEmail)
		{
			var lead = _unitOfWork.LeadsRepository.Get()
				//.Include(e => e.Phones)
				//.Include(l => l.Emails)
				.FirstOrDefault(l => l.Id == model.Id && l.User == null);

			// Проверка на неправильный адресс
			model.NewCustomer.Addresses.RemoveAll(e => string.IsNullOrWhiteSpace(e.Line1) || string.IsNullOrWhiteSpace(e.Line2));

			// Мапим нашего нового юзера(Телефоны обнуляем, так как запишет дубыль)

			var newCustomer = Mapper.Map<UserViewModel, User>(model.NewCustomer);

			newCustomer.UserTypeId = (int)UserType.Customer;
			newCustomer.Phones = new List<Phone>();
			newCustomer.Email = lead.Email;

			// Добавляем новые телефоны
			if (newPhones != null && newPhones.Count != 0)
			{
				var newPhoneAfterMap = Mapper.Map<List<PhoneViewModel>, List<Phone>>(newPhones);
				newPhoneAfterMap = newPhoneAfterMap.Where(e => !string.IsNullOrWhiteSpace(e.PhoneNumber))
					.ToList();

				foreach (var phone in newPhoneAfterMap)
				{
					newCustomer.Phones.Add(phone);
				}
			}

			// Добавляем новые адресса
			if (newAddress != null && newAddress.Count != 0)
			{
				var newAddressAfterMap = Mapper.Map<List<AddressViewModel>, List<Address>>(newAddress);
				newAddressAfterMap = newAddressAfterMap.Where(e => !(string.IsNullOrWhiteSpace(e.Line1) || string.IsNullOrWhiteSpace(e.Line2)))
					.ToList();

				foreach (var address in newAddressAfterMap)
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

			_unitOfWork.UsersRepository.Create(newCustomer);


			// Поле того как получили айди новосозданного юзера, прописываем его в телефон который был прикреплен к лиду

			var leadPhones = _unitOfWork.PhonesRepository.Get(e => e.LeadId == model.Id);

			foreach (var phone in model.NewCustomer.Phones)
			{
				var leadPhone = leadPhones.FirstOrDefault(e => e.Id == phone.Id);
				leadPhone = Mapper.Map<PhoneViewModel, Phone>(phone, leadPhone);
				leadPhone.User = newCustomer;
			}

			// прикріплюємо імейли до новоствореного юзера.
			var leadEmails = _unitOfWork.EmailsRepository.Get(e => e.LeadId == model.Id);

			foreach (var email in leadEmails)
			{
				email.User = newCustomer;
			}

			// прикрипляем записи лида к ново созданному юзеру

			//context.Notes
			//    .Where(e => e.LeadId == model.Id)
			//    .ForEachAsync(e => e.UserId = newCustomerInDB.Id)
			//    .Wait();

			var notes = _unitOfWork.NotesRepository.Get(e => e.LeadId == model.Id);
			notes.ForEach(e => e.User = newCustomer);
			_unitOfWork.NotesRepository.UpdateRange(notes);

			// проставляем что лида перевели в кастомера
			lead.IsConverted = true;
            lead.IsSaved = true;
            _unitOfWork.LeadsRepository.Update(lead);

            // логирование процесса конвертации

            _unitOfWork.LeadsConvertedLogsRepository.Create(new LeadConvertedLog
			{
				Lead = lead,
				User = newCustomer,
				ConvertDateTime = DateTime.Now,
				ConvertedByUserId = _unitOfWork.UsersRepository.FindBy(e => e.Email == currentUserEmail).Id
			});           
            
            _unitOfWork.Save();
		}
	}
}