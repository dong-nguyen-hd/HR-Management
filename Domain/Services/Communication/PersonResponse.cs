namespace HR_Management.Domain.Services.Communication
{
    public class PersonResponse<T> : BaseResponse<T>
    {
        public PersonResponse(T resource) : base(resource) { }

        public PersonResponse(string message) : base(message) { }
    }
}
