namespace HR_Management.Domain.Services.Communication
{
    public class AccountResponse<T> : PaginationResponse<T>
    {
        public AccountResponse(T resource) : base(resource) { }

        public AccountResponse(string message) : base(message) { }
    }
}
