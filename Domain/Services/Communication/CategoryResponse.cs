using HR_Management.Domain.Models;

namespace HR_Management.Domain.Services.Communication
{
    public class CategoryResponse : BaseResponse<Category>
    {
        public CategoryResponse(bool isSuccess) : base(isSuccess) { }

        public CategoryResponse(Category category) : base(category) { }

        public CategoryResponse(object objCategory) : base(objCategory) { }

        public CategoryResponse(string message) : base(message) { }
    }
}
