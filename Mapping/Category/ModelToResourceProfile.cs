using AutoMapper;
using HR_Management.Resources.Category;

namespace HR_Management.Mapping.Category
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.Category, CategoryResource>();
        }
    }
}
