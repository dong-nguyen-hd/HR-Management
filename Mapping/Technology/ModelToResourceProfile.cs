using AutoMapper;
using HR_Management.Resources.Technology;

namespace HR_Management.Mapping.Technology
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.Technology, TechnologyResource>();
        }
    }
}
