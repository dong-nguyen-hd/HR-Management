using AutoMapper;
using Business.Resources.Office;

namespace Business.Mapping.Office
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.Office, OfficeResource>();
        }
    }
}
