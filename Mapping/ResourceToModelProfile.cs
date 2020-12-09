using AutoMapper;
using HR_Management.Domain.Models;
using HR_Management.Resources;

namespace HR_Management.Mapping
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateEducationResource, Education>();
        }
        
    }
}
