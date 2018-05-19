using AutoMapper;
using CRM.DAL.Entities;
using CRM.Enums;
using CRM.Models;
using System.Linq;

namespace CRM.AutoMapper
{
	public static class AutoMapperConfiguration
	{
		public static void Configure()
		{
			Mapper.Initialize(cfg =>
			{
				cfg.CreateMap<Phone, PhoneViewModel>()
					.ForMember(p => p.Type, opt => opt.MapFrom(t => (PhoneType?)t.TypeId));

				cfg.CreateMap<PhoneViewModel, Phone>()
					//.ForMember(p => p.Id, opt => opt.Ignore())
					.ForMember(p => p.Type, opt => opt.Ignore())
					.ForMember(p => p.TypeId, opt => opt.MapFrom(i => i.Type));

				cfg.CreateMap<Address, AddressViewModel>()
					.ForMember(p => p.Type, opt => opt.MapFrom(a => (AddressType?)a.AddressTypeId));

				cfg.CreateMap<AddressViewModel, Address>()
					//.ForMember(p => p.Id, opt => opt.Ignore())
					.ForMember(p => p.AddressType, opt => opt.Ignore())
					.ForMember(p => p.AddressTypeId, opt => opt.MapFrom(e => (int?)e.Type));

				cfg.CreateMap<User, UserViewModel>()
					.ForMember(i => i.Role, opt => opt.MapFrom(u => (UserRole)u.Role));

				cfg.CreateMap<UserViewModel, User>();
				//.ForMember(c => c.Lead, opt => opt.Ignore());

				cfg.CreateMap<Lead, LeadViewModel>();
				//.ForMember(l => l.Phones, opt => opt
				//    .MapFrom(i => new List<PhoneViewModel>() { new PhoneViewModel() { PhoneNumber = i.Phones.FirstOrDefault(p => p.TypeId == (int)PhoneType.HomePhone).PhoneNumber } }));

				cfg.CreateMap<LeadViewModel, Lead>();
				//.ForMember(l => l.Phones, opt => opt
				//    .MapFrom(i => new List<Phone> { new Phone { PhoneNumber = i.Phones.FirstOrDefault().PhoneNumber, TypeId = (int)PhoneType.HomePhone } }));

				//cfg.CreateMap<Lead, LeadViewModel>()
				//    .AfterMap((l, lvm) => lvm.Phone = l.Phones.FirstOrDefault()?.PhoneNumber);

				//cfg.CreateMap<LeadViewModel, Lead>()
				//    .ForMember(l => l.Id, opt => opt.Ignore())
				//    .AfterMap((l, opt) => opt.Phones.Add(new Phone { PhoneNumber = l.Phone }));

				//cfg.CreateMap<Customer, CustomerViewModel>();
				//cfg.CreateMap<CustomerViewModel, Customer>()
				//    .ForMember(c => c.Lead, opt => opt.Ignore());

				// Временно, потому нужно переделать связь между юзерами и записями
				cfg.CreateMap<Note, NoteViewModel>();
				cfg.CreateMap<NoteViewModel, Note>();

				cfg.CreateMap<EmailViewModel, Email>()
					.ForMember(e => e.Text, opt => opt.MapFrom(i => i.Body));

				cfg.CreateMap<Email, EmailViewModel>()
					.ForMember(e => e.Body, opt => opt.MapFrom(i => i.Text))
					.ForMember(e => e.From, opt => opt.Ignore())
					.ForMember(e => e.To, opt => opt.Ignore());

				cfg.CreateMap<User, RegisterViewModel>()
					.ForMember(r => r.ConfirmPassword, u => u.Ignore());

				cfg.CreateMap<RegisterViewModel, User>()
					.ForMember(e => e.Id, opt => opt.Ignore())
					.ForMember(e => e.Title, opt => opt.Ignore())
					.ForMember(e => e.Role, opt => opt.UseValue((int)UserRole.AdminStaff))
					.ForMember(e => e.UserTypeId, opt => opt.UseValue((int)UserType.AdminTeamMember))
					.ForMember(e => e.UserType, opt => opt.Ignore())
					.ForMember(e => e.Phones, opt => opt.Ignore())
					.ForMember(e => e.Addresses, opt => opt.Ignore())
					.ForMember(e => e.Notes, opt => opt.Ignore())
					.ForMember(e => e.Emails, opt => opt.Ignore())
					.ForMember(e => e.Calls, opt => opt.Ignore());

				cfg.CreateMap<Lead, CreateLeadViewModel>()
					.ForMember(e => e.FirstName, opt => opt.Ignore())
					.ForMember(e => e.LastName, opt => opt.Ignore())
					.ForMember(e => e.Phone, opt => opt.MapFrom(i => i.Phones.FirstOrDefault()));

				cfg.CreateMap<CreateLeadViewModel, Lead>()
					.ForMember(
						e => e.Phones,
						opt => opt.MapFrom(i => i.Phones.Select(p => new Phone
						{
							PhoneNumber = p
						})))
					.ForMember(e => e.Discipline, opt => opt.Ignore())
					.ForMember(e => e.AgeGroup, opt => opt.Ignore())
					.ForMember(e => e.Status, opt => opt.Ignore())
					.ForMember(e => e.StatusNotes, opt => opt.Ignore())
					.ForMember(e => e.IssueRaised, opt => opt.Ignore())
					.ForMember(e => e.LeadOwner, opt => opt.Ignore())
					.ForMember(e => e.IsConverted, opt => opt.Ignore())
					.ForMember(e => e.User, opt => opt.Ignore())
					.ForMember(e => e.Emails, opt => opt.Ignore())
					.ForMember(e => e.Calls, opt => opt.Ignore())

					.ForMember(e => e.Id, opt => opt.Ignore());


			});
		}
	}
}