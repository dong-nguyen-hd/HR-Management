namespace HR_Management.Domain.Services.Communication
{
    public class InformationResponse<T> : PaginationResponse<T>
    {
        public InformationResponse(T resource) : base(resource) { }

        public InformationResponse(string message) : base(message) { }
    }
}
