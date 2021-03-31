using AutoMapper;
using HR_Management.Domain.Services;
using HR_Management.Resources.Information;
using HR_Management.Resources.Person;
using System;
using System.Collections.Generic;

namespace HR_Management.Mapping.Person
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.Person, PersonResource>()
                .ForMember(x => x.Avatar, opt => opt.MapFrom<CustomResolver>())
                .ForMember(x => x.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(x => x.OrderIndex, opt => opt.MapFrom(src => ConvertList(src.OrderIndex)));

            CreateMap<Domain.Models.Person, InformationResource>()
                .ForPath(x => x.Person.OrderIndex, opt => opt.Ignore())
                .ForPath(x => x.Person.Location, opt => opt.Ignore())
                .ForMember(x => x.Person, opt => opt.MapFrom(src => src))
                .ForPath(x => x.Person.OrderIndex, opt => opt.MapFrom(src => ConvertList(src.OrderIndex)))
                .ForPath(x => x.Person.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(x => x.WorkHistory, opt => opt.MapFrom(src => src.WorkHistories))
                .ForMember(x => x.Education, opt => opt.MapFrom(src => src.Educations))
                .ForMember(x => x.Certificate, opt => opt.MapFrom(src => src.Certificates))
                .ForMember(x => x.CategoryPerson, opt => opt.MapFrom(src => src.CategoryPersons))
                .ForMember(x => x.Project, opt => opt.MapFrom(src => src.Projects));
        }

        List<int> ConvertList(string resource)
        {
            List<int> tempList = new List<int>();
            string[] temp = resource.Split(',');

            foreach (var number in temp)
            {
                try
                {
                    tempList.Add(Convert.ToInt32(number.Trim()));
                }
                catch
                {
                    continue;
                }
            }

            return tempList;
        }
    }

    /// <summary>
    /// Custom Value Resolvers
    /// </summary>
    class CustomResolver : IValueResolver<Domain.Models.Person, PersonResource, Uri>
    {
        private readonly IUriService _uriService;

        public CustomResolver(IUriService uriService)
        {
            this._uriService = uriService;
        }

        public Uri Resolve(Domain.Models.Person source, PersonResource destination, Uri destMember, ResolutionContext context)
            => string.IsNullOrEmpty(source.Avatar) ? null : _uriService.GetRouteUri($"{Startup.ImagePathMobile}{source.Avatar}");
    }
}
