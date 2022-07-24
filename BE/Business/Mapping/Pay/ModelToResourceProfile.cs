using AutoMapper;
using Business.Resources.Pay;

namespace Business.Mapping.Pay
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.Pay, PayResource>();
        }
    }
}
