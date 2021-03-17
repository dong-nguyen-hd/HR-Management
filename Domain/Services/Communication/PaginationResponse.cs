using System;

namespace HR_Management.Domain.Services.Communication
{
    public abstract class PaginationResponse<T> : BaseResponse<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }

        public PaginationResponse(T resource) : base(resource) { }

        public PaginationResponse(string message) : base(message) { }
    }
}
