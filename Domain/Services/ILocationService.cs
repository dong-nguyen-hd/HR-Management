using HR_Management.Domain.Services.Communication;
using HR_Management.Resources.Location;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface ILocationService
    {
        Task<LocationResponse<IEnumerable<LocationResource>>> ListAsync();
        Task<LocationResponse<LocationResource>> CreateAsync(CreateLocationResource resource);
        Task<LocationResponse<LocationResource>> UpdateAsync(int id, CreateLocationResource resource);
        Task<LocationResponse<LocationResource>> DeleteAsync(int id);
    }
}
