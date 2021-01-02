using HR_Management.Domain.Models;
using HR_Management.Domain.Services.Communication;
using HR_Management.Resources;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface IProjectService
    {
        Task<ProjectResponse> ListAsync(int personId);
        Task<ProjectResponse> CreateAsync(Project project);
        Task<ProjectResponse> UpdateAsync(int id, Project project);
        Task<ProjectResponse> DeleteAsync(int id);
        Task<ProjectResponse> SwapAsync(SwapResource obj);
    }
}
