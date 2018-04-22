using AutoMapper;
using CRM.Enums;
using CRM.Models;
using CRMData.Entities;
using System.Collections.Generic;
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
                    .ForMember(p => p.Type, opt => opt.MapFrom(t => (PhoneType)t.TypeId));

                cfg.CreateMap<PhoneViewModel, Phone>()
                    .ForMember(p => p.Id, opt => opt.Ignore())
                    .ForMember(p => p.Type, opt => opt.MapFrom(i => new DPhoneType
                    {
                        Id = (int)i.Type
                    }));

                cfg.CreateMap<Address, AddressViewModel>()
                    .ForMember(p => p.Type, opt => opt.MapFrom(a => (AddressType?)a.AddressTypeId));

                cfg.CreateMap<AddressViewModel, Address>()
                    //.ForMember(p => p.Id, opt => opt.Ignore())
                    .ForMember(p => p.AddressTypeId, opt => opt.MapFrom(e => (int?)e.Type));

                cfg.CreateMap<User, UserViewModel>()
                    .ForMember(i => i.Role, opt => opt.MapFrom(u => (UserRole)u.Role));

                cfg.CreateMap<Lead, LeadViewModel>()
                .ForMember(l => l.Phones, opt => opt
                    .MapFrom(i => new List<PhoneViewModel>() { new PhoneViewModel() { PhoneNumber = i.Phones.FirstOrDefault(p => p.TypeId == (int)PhoneType.HomePhone).PhoneNumber } }));

                cfg.CreateMap<LeadViewModel, Lead>()
                    .ForMember(l => l.Phones, opt => opt
                        .MapFrom(i => new List<Phone> { new Phone { PhoneNumber = i.Phones.FirstOrDefault().PhoneNumber, TypeId = (int)PhoneType.HomePhone } }));

                //cfg.CreateMap<Lead, LeadViewModel>()
                //    .AfterMap((l, lvm) => lvm.Phone = l.Phones.FirstOrDefault()?.PhoneNumber);

                //cfg.CreateMap<LeadViewModel, Lead>()
                //    .ForMember(l => l.Id, opt => opt.Ignore())
                //    .AfterMap((l, opt) => opt.Phones.Add(new Phone { PhoneNumber = l.Phone }));

                cfg.CreateMap<Customer, CustomerViewModel>();
                cfg.CreateMap<CustomerViewModel, Customer>()
                    .ForMember(c => c.Id, opt => opt.Ignore());

                // Временно, потому нужно переделать связь между юзерами и записями
                cfg.CreateMap<Note, NoteViewModel>();
            });
        }
    }
}