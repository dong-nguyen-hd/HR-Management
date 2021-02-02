namespace HR_Management.Domain.Services.Communication
{
    public class LocationResponse<T> : BaseResponse<T>
    {
        public LocationResponse(T resource) : base(resource) { }

        public LocationResponse(string message) : base(message) { }
    }
}
