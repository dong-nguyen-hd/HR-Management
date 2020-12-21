using AutoMapper;
using HR_Management.Resources.Education;

namespace HR_Management.Mapping.Education
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateEducationResource, Domain.Models.Education>();
            CreateMap<EducationResource, Domain.Models.Education>();
            CreateMap<UpdateEducationResource, Domain.Models.Education>();
        }
    }
}
