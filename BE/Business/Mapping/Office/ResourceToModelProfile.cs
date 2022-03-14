using AutoMapper;
using Business.Extensions;
using Business.Resources.Office;

namespace Business.Mapping.Office
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateOfficeResource, Domain.Models.Office>()
                .ForMember(x => x.Address, opt => opt.MapFrom(src => src.Address.RemoveSpaceCharacter()))
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name.RemoveSpaceCharacter()));

            CreateMap<UpdateOfficeResource, Domain.Models.Office>()
                .ForMember(x => x.Address, opt => opt.MapFrom(src => src.Address.RemoveSpaceCharacter()))
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name.RemoveSpaceCharacter()));
        }
    }
}
