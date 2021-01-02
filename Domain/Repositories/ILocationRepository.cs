using HR_Management.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Domain.Repositories
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>> ListAsync();
        Task AddAsync(Location location);
        void Update(Location location);
        Task<Location> FindByIdAsync(int id);
        void Remove(Location education);
    }
}
