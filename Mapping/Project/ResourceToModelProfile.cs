using AutoMapper;
using HR_Management.Resources.Project;

namespace HR_Management.Mapping.Project
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateProjectResource, Domain.Models.Project>();
            CreateMap<ProjectResource, Domain.Models.Project>();
            CreateMap<UpdateProjectResource, Domain.Models.Project>();
        }
    }
}
