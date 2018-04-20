﻿using AutoMapper;
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
                    .ForMember(p => p.Type, opt => opt.MapFrom(t => (PhoneType)t.Type.TypeName));

                cfg.CreateMap<PhoneViewModel, Phone>()
                    .ForMember(p => p.Id, opt => opt.Ignore())
                    .ForMember(p => p.Type, opt => opt.MapFrom(i => new DPhoneType
                    {
                        Id = (int)i.Type
                    }));

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
                //.ForMember(l => l.Phones, opt => opt
                //    .MapFrom(i => i.Phones.FirstOrDefault(p => p.Type.TypeName == (int)PhoneType.HomePhone).PhoneNumber));

                cfg.CreateMap<LeadViewModel, Lead>();
                    //.ForMember(l => l.Phones, opt => opt
                    //    .MapFrom(i => new List<Phone> { new Phone { PhoneNumber = i.Phones, Type = new DPhoneType { TypeName =  (int)PhoneType.HomePhone} } }));


                cfg.CreateMap<Customer, CustomerViewModel>();
                cfg.CreateMap<CustomerViewModel, Customer>()
                    .ForMember(c => c.Id, opt => opt.Ignore());
            });
        }
    }
}