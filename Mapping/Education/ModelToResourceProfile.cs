using AutoMapper;
using HR_Management.Resources.Education;

namespace HR_Management.Mapping.Education
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.Education, EducationResource>();
        }
    }
}
