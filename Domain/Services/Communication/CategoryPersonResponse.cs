namespace HR_Management.Domain.Services.Communication
{
    public class CategoryPersonResponse<T> : BaseResponse<T>
    {
        public CategoryPersonResponse(T categoryPerson) : base(categoryPerson) { }

        public CategoryPersonResponse(string message) : base(message) { }
    }
}
