using HR_Management.Domain.Models;
using HR_Management.Domain.Services.Communication;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface ILocationService
    {
        Task<LocationResponse> ListAsync();
        Task<LocationResponse> CreateAsync(Location location);
        Task<LocationResponse> UpdateAsync(int id, Location location);
        Task<LocationResponse> DeleteAsync(int id);
    }
}
