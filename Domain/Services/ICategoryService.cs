using HR_Management.Domain.Models;
using HR_Management.Domain.Services.Communication;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface ICategoryService
    {
        Task<CategoryResponse> ListAsync();
        Task<CategoryResponse> CreateAsync(Category category);
        Task<CategoryResponse> UpdateAsync(int id, Category category);
        Task<CategoryResponse> DeleteAsync(int id);
    }
}
