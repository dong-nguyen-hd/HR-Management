using HR_Management.Domain.Services.Communication;
using HR_Management.Resources;
using HR_Management.Resources.CategoryPerson;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface ICategoryPersonService
    {
        Task<CategoryPersonResponse<IEnumerable<CategoryPersonResource>>> ListAsync(int personId);
        Task<CategoryPersonResponse<CategoryPersonResource>> CreateAsync(CreateCategoryPersonResource createCategoryPersonResource);
        Task<CategoryPersonResponse<CategoryPersonResource>> UpdateAsync(int id, UpdateCategoryPersonResource updateCategoryPersonResource);
        Task<CategoryPersonResponse<CategoryPersonResource>> DeleteAsync(int id);
        Task<CategoryPersonResponse<CategoryPersonResource>> SwapAsync(SwapResource obj);
    }
}
