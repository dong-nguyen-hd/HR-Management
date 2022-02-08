using AutoMapper;
using Business.Domain.Services;
using Business.Resources.Account;
using Business.Resources.Authentication;
using Business.Resources.Information;
using Microsoft.Extensions.Options;
using System;

namespace Business.Mapping.Account
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.Account, AccountResource>()
                .ForMember(x => x.Avatar, opt => opt.MapFrom<CustomResolver>());

            CreateMap<Domain.Models.Account, AccessTokenResource>();
        }
    }

    /// <summary>
    /// Custom Value Resolvers
    /// </summary>
    class CustomResolver : IValueResolver<Domain.Models.Account, AccountResource, Uri>
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
        public Uri Resolve(Domain.Models.Account source, AccountResource destination, Uri destMember, ResolutionContext context)
            => string.IsNullOrEmpty(source.Avatar) ? null : _uriService.GetRouteUri($"{_hostResource.ThumbnailImagePath}{source.Avatar}");
        #endregion
    }
}
