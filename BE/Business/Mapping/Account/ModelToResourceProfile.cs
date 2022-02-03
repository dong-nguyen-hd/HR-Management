using AutoMapper;
using Business.Resources.Account;
using Business.Resources.Authentication;

namespace Business.Mapping.Account
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.Account, AccountResource>();

            CreateMap<Domain.Models.Account, AccessTokenResource>();
        }
    }
}
