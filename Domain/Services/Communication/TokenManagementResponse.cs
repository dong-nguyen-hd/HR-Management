namespace HR_Management.Domain.Services.Communication
{
    public class TokenManagementResponse<T> : BaseResponse<T>
    {
        public TokenManagementResponse(T resource) : base(resource) { }

        public TokenManagementResponse(string message) : base(message) { }
    }
}
