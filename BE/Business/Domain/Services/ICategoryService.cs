using Business.Domain.Models;
using Business.Resources.Category;

namespace Business.Domain.Services
{
    public interface ICategoryService : IBaseService<CategoryResource, CreateCategoryResource, UpdateCategoryResource, Category>
    {
    }
}
