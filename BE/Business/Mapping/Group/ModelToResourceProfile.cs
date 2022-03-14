using AutoMapper;
using Business.Resources.Group;

namespace Business.Mapping.Group
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.Group, GroupResource>()
                .ForMember(x => x.Technology, opt => opt.Ignore());
        }
    }
}
