using AutoMapper;
using Business.Extensions;
using Business.Resources.Location;

namespace Business.Mapping.Location
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateLocationResource, Domain.Models.Location>()
                .ForMember(x => x.Address, opt => opt.MapFrom(src => src.Address.RemoveSpaceCharacter()))
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name.RemoveSpaceCharacter()));

            CreateMap<UpdateLocationResource, Domain.Models.Location>()
                .ForMember(x => x.Address, opt => opt.MapFrom(src => src.Address.RemoveSpaceCharacter()))
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name.RemoveSpaceCharacter()));
        }
    }
}
