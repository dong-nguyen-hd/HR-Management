using AutoMapper;
using HR_Management.Extensions;
using HR_Management.Resources.Project;

namespace HR_Management.Mapping.Project
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateProjectResource, Domain.Models.Project>()
                .ForMember(x => x.Technology, opt => opt.Ignore())
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name.RemoveSpaceCharacter()))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description.RemoveSpaceCharacter()))
                .ForMember(x => x.Position, opt => opt.MapFrom(src => src.Position.RemoveSpaceCharacter()))
                .ForMember(x => x.Responsibilities, opt => opt.MapFrom(src => src.Responsibilities.RemoveSpaceCharacter()));

            CreateMap<ProjectResource, Domain.Models.Project>()
                .ForMember(x => x.Technology, opt => opt.Ignore())
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name.RemoveSpaceCharacter()))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description.RemoveSpaceCharacter()))
                .ForMember(x => x.Position, opt => opt.MapFrom(src => src.Position.RemoveSpaceCharacter()))
                .ForMember(x => x.Responsibilities, opt => opt.MapFrom(src => src.Responsibilities.RemoveSpaceCharacter()));

            CreateMap<UpdateProjectResource, Domain.Models.Project>()
                .ForMember(x => x.Technology, opt => opt.Ignore())
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name.RemoveSpaceCharacter()))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description.RemoveSpaceCharacter()))
                .ForMember(x => x.Position, opt => opt.MapFrom(src => src.Position.RemoveSpaceCharacter()))
                .ForMember(x => x.Responsibilities, opt => opt.MapFrom(src => src.Responsibilities.RemoveSpaceCharacter()));
        }
    }
}
