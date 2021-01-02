using AutoMapper;
using HR_Management.Resources.CategoryPerson;

namespace HR_Management.Mapping.CategoryPerson
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateCategoryPersonResource, Domain.Models.CategoryPerson>();
            CreateMap<CategoryPersonResource, Domain.Models.CategoryPerson>();
            CreateMap<UpdateCategoryPersonResource, Domain.Models.CategoryPerson>();
        }
    }
}
