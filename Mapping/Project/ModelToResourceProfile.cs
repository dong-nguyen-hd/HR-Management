using AutoMapper;
using HR_Management.Resources.Project;

namespace HR_Management.Mapping.Project
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.Project, ProjectResource>().ForMember(x => x.Technology, opt => opt.Ignore());
        }
    }
}
