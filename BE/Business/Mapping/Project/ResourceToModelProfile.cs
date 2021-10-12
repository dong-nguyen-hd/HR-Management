using AutoMapper;
using Business.Extensions;
using Business.Resources.Project;

namespace Business.Mapping.Project
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateProjectResource, Domain.Models.Project>()
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name.RemoveSpaceCharacter()))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description.RemoveSpaceCharacter()))
                .ForMember(x => x.Position, opt => opt.MapFrom(src => src.Position.RemoveSpaceCharacter()))
                .ForMember(x => x.Technology, opt => opt.MapFrom(src => src.Technology.ConcatenateWithComma()))
                .ForMember(x => x.Responsibilities, opt => opt.MapFrom(src => src.Responsibilities.RemoveSpaceCharacter()));

            CreateMap<UpdateProjectResource, Domain.Models.Project>()
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name.RemoveSpaceCharacter()))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description.RemoveSpaceCharacter()))
                .ForMember(x => x.Position, opt => opt.MapFrom(src => src.Position.RemoveSpaceCharacter()))
                .ForMember(x => x.Technology, opt => opt.MapFrom(src => src.Technology.ConcatenateWithComma()))
                .ForMember(x => x.Responsibilities, opt => opt.MapFrom(src => src.Responsibilities.RemoveSpaceCharacter()));
        }
    }
}
