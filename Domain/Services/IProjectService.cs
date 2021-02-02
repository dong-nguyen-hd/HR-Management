using HR_Management.Domain.Services.Communication;
using HR_Management.Resources;
using HR_Management.Resources.Project;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface IProjectService
    {
        Task<ProjectResponse<IEnumerable<ProjectResource>>> ListAsync(int personId);
        Task<ProjectResponse<ProjectResource>> CreateAsync(CreateProjectResource createProjectResource);
        Task<ProjectResponse<ProjectResource>> UpdateAsync(int id, UpdateProjectResource updateProjectResource);
        Task<ProjectResponse<ProjectResource>> DeleteAsync(int id);
        Task<ProjectResponse<ProjectResource>> SwapAsync(SwapResource obj);
    }
}
