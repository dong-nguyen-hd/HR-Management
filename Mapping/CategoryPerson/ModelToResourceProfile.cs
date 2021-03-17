#nullable enable
using AutoMapper;
using HR_Management.Domain.Services;
using HR_Management.Resources.CategoryPerson;
using HR_Management.Resources.Technology;
using System.Collections.Generic;

namespace HR_Management.Mapping.CategoryPerson
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.CategoryPerson, CategoryPersonResource>()
                .ForMember(x => x.Technology, opt => opt.MapFrom<CustomResolver>());
        }
    }

    /// <summary>
    /// Custom Value Resolvers
    /// </summary>
    class CustomResolver : IValueResolver<Domain.Models.CategoryPerson, CategoryPersonResource, List<TechnologyResource>>
    {
        private readonly ITechnologyService _technologyService;

        public CustomResolver(ITechnologyService technologyService)
        {
            this._technologyService = technologyService;
        }

        public List<TechnologyResource> Resolve(Domain.Models.CategoryPerson source, CategoryPersonResource destination, List<TechnologyResource> destMember, ResolutionContext context)
            => _technologyService.ConvertListAsync(source.Technology).Result;
    }
}
