using AutoMapper;
using Business.Resources.Project;

namespace Business.Mapping.Project
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.Project, ProjectResource>()
                .ForMember(x => x.Technology, opt => opt.Ignore());
        }
    }
}
