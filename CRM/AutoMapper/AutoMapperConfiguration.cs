using AutoMapper;
using CRM.Enums;
using CRM.Models;
using CRMData.Entities;

namespace CRM.AutoMapper
{
    public static class ModelsMapper
    {
        public static void CreateRules()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<User, UserViewModel>()
                    .ForMember("Role", opt => opt.MapFrom(c => (UserRole)c.Role));

                cfg.CreateMap<Lead, LeadViewModel>();
                cfg.CreateMap<LeadViewModel, Lead>()
                    .ForMember(l => l.LeadId, opt => opt.Ignore());
            });
        }
    }
}