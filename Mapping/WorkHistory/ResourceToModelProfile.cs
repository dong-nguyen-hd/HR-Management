using AutoMapper;
using HR_Management.Extensions;
using HR_Management.Resources.WorkHistory;

namespace HR_Management.Mapping.WorkHistory
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateWorkHistoryResource, Domain.Models.WorkHistory>()
                .ForMember(x => x.Position, opt => opt.MapFrom(src => src.Position.RemoveSpaceCharacter()))
                .ForMember(x => x.CompanyName, opt => opt.MapFrom(src => src.CompanyName.RemoveSpaceCharacter()));

            CreateMap<WorkHistoryResource, Domain.Models.WorkHistory>()
                .ForMember(x => x.Position, opt => opt.MapFrom(src => src.Position.RemoveSpaceCharacter()))
                .ForMember(x => x.CompanyName, opt => opt.MapFrom(src => src.CompanyName.RemoveSpaceCharacter()));

            CreateMap<UpdateWorkHistoryResource, Domain.Models.WorkHistory>()
                .ForMember(x => x.Position, opt => opt.MapFrom(src => src.Position.RemoveSpaceCharacter()))
                .ForMember(x => x.CompanyName, opt => opt.MapFrom(src => src.CompanyName.RemoveSpaceCharacter()));
        }
    }
}
