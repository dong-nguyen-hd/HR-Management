using AutoMapper;
using HR_Management.Extensions;
using HR_Management.Infrastructure;
using HR_Management.Resources.Account;
using System;

namespace HR_Management.Mapping.Account
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateAccountResource, Domain.Models.Account>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(src => src.UserName.ToLower()))
                .ForMember(x => x.Password, opt => opt.MapFrom(src => src.Password.HashingPassword(10000)))
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name.RemoveSpaceCharacter()))
                .ForMember(x => x.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(x => x.LastActivity, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(x => x.Role, opt => opt.MapFrom(src => GetRole(src.Role)));

            CreateMap<UpdateAccountResource, Domain.Models.Account>()
                .ForMember(x => x.Password, opt => opt.MapFrom(src => src.Password.HashingPassword(10000)))
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name.RemoveSpaceCharacter()))
                .ForMember(x => x.Role, opt => opt.MapFrom(src => GetRole(src.Role)));
        }

        private static string GetRole(int @enum)
        {
            eRole roleEnum = (eRole)@enum;

            return roleEnum.ToDescriptionString();
        }
    }
}
