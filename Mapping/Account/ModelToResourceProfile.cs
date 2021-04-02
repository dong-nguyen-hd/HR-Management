using AutoMapper;
using HR_Management.Resources.Account;
using HR_Management.Resources.Authentication;

namespace HR_Management.Mapping.Account
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.Account, AccountResource>();

            CreateMap<Domain.Models.Account, TokenResource>();
        }
    }
}
