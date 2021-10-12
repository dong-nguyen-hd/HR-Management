using AutoMapper;
using Business.Resources.CategoryPerson;

namespace Business.Mapping.CategoryPerson
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.CategoryPerson, CategoryPersonResource>()
                .ForMember(x => x.Technology, opt => opt.Ignore());
        }
    }
}
