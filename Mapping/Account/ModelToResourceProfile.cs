using AutoMapper;
using HR_Management.Resources.Account;

namespace HR_Management.Mapping.Account
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.Account, AccountResource>();
        }
    }
}
