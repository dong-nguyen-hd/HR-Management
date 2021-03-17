using AutoMapper;
using HR_Management.Extensions;
using HR_Management.Resources.CategoryPerson;

namespace HR_Management.Mapping.CategoryPerson
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateCategoryPersonResource, Domain.Models.CategoryPerson>()
                .ForMember(x => x.Technology, opt => opt.MapFrom(src => src.Technology.ConcatenateWithComma()));

            CreateMap<CategoryPersonResource, Domain.Models.CategoryPerson>();
                
            CreateMap<UpdateCategoryPersonResource, Domain.Models.CategoryPerson>()
                .ForMember(x => x.Technology, opt => opt.MapFrom(src => src.Technology.ConcatenateWithComma()));
        }
    }
}
