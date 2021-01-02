using HR_Management.Domain.Models;
using HR_Management.Domain.Services.Communication;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface ITechnologyService
    {
        Task<TechnologyResponse> ListAsync();
        Task<TechnologyResponse> ListAsync(int categoryId);
        Task<TechnologyResponse> CreateAsync(Technology technology);
        Task<TechnologyResponse> UpdateAsync(int id, Technology technology);
        Task<TechnologyResponse> DeleteAsync(int id);
    }
}
