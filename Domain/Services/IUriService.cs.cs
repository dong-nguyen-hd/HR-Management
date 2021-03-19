using HR_Management.Resources.Queries;
using System;

namespace HR_Management.Domain.Services
{
    public interface IUriService
    {
        Uri GetPageUri(QueryResource pagination, string route);
        Uri GetRouteUri(string route);
    }
}
