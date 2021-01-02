using AutoMapper;
using HR_Management.Resources.Category;

namespace HR_Management.Mapping.Category
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateCategoryResource, Domain.Models.Category>();
            CreateMap<CategoryResource, Domain.Models.Category>();
        }
    }
}
