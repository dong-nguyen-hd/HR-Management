using HR_Management.Domain.Services;
using HR_Management.Domain.Services.Communication;
using HR_Management.Resources.Queries;
using System;

namespace HR_Management.Extensions
{
    public static class RelatePagination
    {
        public static void CreatePaginationResponse<T, T1>(this T response, 
            QueryResource pagination, 
            int totalRecords, 
            IUriService uriService, 
            string route) where T : PaginationResponse<T1>
        {
            // Assign Query-Resource
            response.Page = pagination.Page;
            response.PageSize = pagination.PageSize;
            // Assign Total-Pages
            var totalPages = ((double)totalRecords / (double)pagination.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            // Assign Next-Page
            response.NextPage =
                pagination.Page >= 1 && pagination.Page < roundedTotalPages
                ? uriService.GetPageUri(new QueryResource(pagination.Page + 1, pagination.PageSize), route)
                : null;
            // Assign Previous-Page
            response.PreviousPage =
                pagination.Page - 1 >= 1 && pagination.Page <= roundedTotalPages
                ? uriService.GetPageUri(new QueryResource(pagination.Page - 1, pagination.PageSize), route)
                : null;
            // Assign First-Page
            response.FirstPage = uriService.GetPageUri(new QueryResource(1, pagination.PageSize), route);
            // Assign Last-Page
            response.LastPage = uriService.GetPageUri(new QueryResource(roundedTotalPages, pagination.PageSize), route);
            // Assign Total-Pages
            response.TotalPages = roundedTotalPages;
            // Assign Total-Records
            response.TotalRecords = totalRecords;
        }
    }
}
