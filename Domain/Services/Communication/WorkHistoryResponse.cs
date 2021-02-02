namespace HR_Management.Domain.Services.Communication
{
    public class WorkHistoryResponse<T> : BaseResponse<T>
    {
        public WorkHistoryResponse(T resource) : base(resource) { }

        public WorkHistoryResponse(string message) : base(message) { }
    }
}
