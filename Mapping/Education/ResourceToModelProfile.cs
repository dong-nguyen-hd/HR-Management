using AutoMapper;
using HR_Management.Resources.Education;
using HR_Management.Domain.Models;

namespace HR_Management.Mapping.Education
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateEducationResource, Domain.Models.Education>();
        }
    }
}
