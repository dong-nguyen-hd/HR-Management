using HR_Management.Domain.Services.Communication;
using HR_Management.Resources.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface ICategoryService
    {
        Task<CategoryResponse<IEnumerable<CategoryResource>>> ListAsync();
        Task<CategoryResponse<CategoryResource>> CreateAsync(CreateCategoryResource resource);
        Task<CategoryResponse<CategoryResource>> UpdateAsync(int id, CreateCategoryResource resource);
        Task<CategoryResponse<CategoryResource>> DeleteAsync(int id);
    }
}
