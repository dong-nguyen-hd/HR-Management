using AutoMapper;
using HR_Management.Extensions;
using HR_Management.Resources.Technology;

namespace HR_Management.Mapping.Technology
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateTechnologyResource, Domain.Models.Technology>()
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name.RemoveSpaceCharacter()));

            CreateMap<TechnologyResource, Domain.Models.Technology>()
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name.RemoveSpaceCharacter()));

            CreateMap<UpdateTechnologyResource, Domain.Models.Technology>()
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name.RemoveSpaceCharacter()));
        }
    }
}
