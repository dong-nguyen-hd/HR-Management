using AutoMapper;
using HR_Management.Resources.Location;

namespace HR_Management.Mapping.Location
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateLocationResource, Domain.Models.Location>();
            CreateMap<LocationResource, Domain.Models.Location>();
        }
    }
}
