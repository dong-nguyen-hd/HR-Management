namespace HR_Management.Domain.Services.Communication
{
    public class CategoryResponse<T> : BaseResponse<T>
    {
        public CategoryResponse(T resource) : base(resource) { }

        public CategoryResponse(string message) : base(message) { }
    }
}
