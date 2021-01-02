using HR_Management.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Domain.Repositories
{
    public interface IWorkHistoryRepository
    {
        Task<IEnumerable<WorkHistory>> ListAsync(int personId);
        Task AddAsync(WorkHistory workHistory);
        void Update(WorkHistory workHistory);
        Task<WorkHistory> FindByIdAsync(int id);
        void Remove(WorkHistory workHistory);
        Task<int> MaximumOrderIndexAsync(int personId);
    }
}
