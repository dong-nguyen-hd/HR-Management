using AutoMapper;
using HR_Management.Resources.Technology;

namespace HR_Management.Mapping.Technology
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateTechnologyResource, Domain.Models.Technology>();
            CreateMap<TechnologyResource, Domain.Models.Technology>();
            CreateMap<UpdateTechnologyResource, Domain.Models.Technology>();
        }
    }
}
