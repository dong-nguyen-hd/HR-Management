using AutoMapper;
using Business.Domain.Services;
using Business.Resources.Information;
using Business.Resources.Person;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace Business.Mapping.Person
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.Person, PersonResource>()
                .ForMember(x => x.Avatar, opt => opt.MapFrom<CustomResolver>())
                .ForMember(x => x.OrderIndex, opt => opt.MapFrom(src => ConvertList(src.OrderIndex)))
                .ForMember(x => x.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(x => x.WorkHistory, opt => opt.MapFrom(src => src.WorkHistories))
                .ForMember(x => x.Education, opt => opt.MapFrom(src => src.Educations))
                .ForMember(x => x.Certificate, opt => opt.MapFrom(src => src.Certificates))
                .ForMember(x => x.CategoryPerson, opt => opt.MapFrom(src => src.CategoryPersons))
                .ForMember(x => x.Project, opt => opt.MapFrom(src => src.Projects));
        }

        private static List<int> ConvertList(string resource)
        {
            List<int> tempList = new();
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
        #region Property
        private readonly IUriService _uriService;
        private readonly HostResource _hostResource;
        #endregion

        #region Constructor
        public CustomResolver(IUriService uriService, IOptionsSnapshot<HostResource> hostResource)
        {
            this._uriService = uriService;
            this._hostResource = hostResource.Value;
        }
        #endregion

        #region Method
        public Uri Resolve(Domain.Models.Person source, PersonResource destination, Uri destMember, ResolutionContext context)
            => string.IsNullOrEmpty(source.Avatar) ? null : _uriService.GetRouteUri($"{_hostResource.ThumbnailImagePath}{source.Avatar}");
        #endregion
    }
}
