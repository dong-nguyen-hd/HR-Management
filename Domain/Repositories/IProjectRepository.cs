using HR_Management.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Domain.Repositories
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> ListAsync(int personId);
        Task AddAsync(Project project);
        void Update(Project project);
        Task<Project> FindByIdAsync(int id);
        void Remove(Project project);
        Task<int> MaximumOrderIndexAsync(int personId);
    }
}
