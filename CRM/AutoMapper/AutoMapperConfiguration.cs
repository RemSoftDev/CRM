using AutoMapper;
using CRM.Enums;
using CRM.Models;
using CRMData.Entities;

namespace CRM.AutoMapper
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Phone, PhoneViewModel>()
                    .ForMember(p => p.Type, opt => opt.MapFrom(t => (PhoneType)t.Type.Id));

                cfg.CreateMap<PhoneViewModel, Phone>()
                    .ForMember(p => p.Id, opt => opt.Ignore());

                cfg.CreateMap<Address, AddressViewModel>()
                    .ForMember(p => p.Type, opt => opt.MapFrom(a => (AddressType)a.Type.Id));

                cfg.CreateMap<AddressViewModel, Address>()
                    .ForMember(p => p.Id, opt => opt.Ignore())
                    .ForMember(p => p.Type, opt => opt.MapFrom(e => new DAddressType
                    {
                        Id = (int)e.Type
                    }));

                cfg.CreateMap<User, UserViewModel>()
                    .ForMember(i => i.Role, opt => opt.MapFrom(u => (UserRole)u.Role));

                cfg.CreateMap<Lead, LeadViewModel>();
                cfg.CreateMap<LeadViewModel, Lead>()
                    .ForMember(l => l.Id, opt => opt.Ignore());

                cfg.CreateMap<Customer, CustomerViewModel>();
                cfg.CreateMap<CustomerViewModel, Customer>()
                    .ForMember(c => c.Id, opt => opt.Ignore());
            });
        }
    }
}