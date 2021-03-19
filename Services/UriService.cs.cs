#nullable enable
using HR_Management.Domain.Services;
using HR_Management.Resources.Queries;
using Microsoft.AspNetCore.WebUtilities;
using System;

namespace HR_Management.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetPageUri(QueryResource pagination, string route)
        {
            var _enpointUri = new Uri(string.Concat(_baseUri, route));
            var modifiedUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "page", pagination.Page.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", pagination.PageSize.ToString());

            return new Uri(modifiedUri);
        }

        public Uri GetRouteUri(string route)
            => new Uri(string.Concat(_baseUri, route));
    }
}
