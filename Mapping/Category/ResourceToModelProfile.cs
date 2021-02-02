using AutoMapper;
using HR_Management.Extensions;
using HR_Management.Resources.Category;

namespace HR_Management.Mapping.Category
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateCategoryResource, Domain.Models.Category>()
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name.RemoveSpaceCharacter()));

            CreateMap<CategoryResource, Domain.Models.Category>()
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name.RemoveSpaceCharacter()));
        }
    }
}
