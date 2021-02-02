using HR_Management.Domain.Services.Communication;
using HR_Management.Resources.Technology;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface ITechnologyService
    {
        Task<TechnologyResponse<IEnumerable<TechnologyResource>>> ListAsync();
        Task<TechnologyResponse<IEnumerable<TechnologyResource>>> ListAsync(int personId);
        Task<TechnologyResponse<TechnologyResource>> CreateAsync(CreateTechnologyResource resource);
        Task<TechnologyResponse<TechnologyResource>> UpdateAsync(int id, UpdateTechnologyResource resource);
        Task<TechnologyResponse<TechnologyResource>> DeleteAsync(int id);
    }
}
