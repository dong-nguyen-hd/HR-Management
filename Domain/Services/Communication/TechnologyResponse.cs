namespace HR_Management.Domain.Services.Communication
{
    public class TechnologyResponse<T> : BaseResponse<T>
    {
        public TechnologyResponse(T resource) : base(resource) { }

        public TechnologyResponse(string message) : base(message) { }
    }
}
