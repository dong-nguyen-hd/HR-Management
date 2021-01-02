using HR_Management.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Domain.Repositories
{
    public interface ITechnologyRepository
    {
        Task<IEnumerable<Technology>> ListAsync();
        Task<IEnumerable<Technology>> ListAsync(int categoryId);
        Task AddAsync(Technology technology);
        void Update(Technology technology);
        Task<Technology> FindByIdAsync(int id);
        void Remove(Technology technology);
    }
}
