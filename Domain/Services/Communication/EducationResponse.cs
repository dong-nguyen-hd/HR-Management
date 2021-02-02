namespace HR_Management.Domain.Services.Communication
{
    public class EducationResponse<T> : BaseResponse<T>
    {
        public EducationResponse(T resource) : base(resource) { }

        public EducationResponse(string message) : base(message) { }
    }
}
