using AutoMapper;
using Business.Resources.Location;

namespace Business.Mapping.Location
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.Location, LocationResource>();
        }
    }
}
