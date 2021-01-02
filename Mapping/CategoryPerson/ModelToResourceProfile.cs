using AutoMapper;
using HR_Management.Resources.CategoryPerson;

namespace HR_Management.Mapping.CategoryPerson
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.CategoryPerson, CategoryPersonResource>();
        }
    }
}
