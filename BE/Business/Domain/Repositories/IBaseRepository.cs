using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Domain.Repositories
{
    public interface IBaseRepository<Entity> where Entity : class
    {
        Task<Entity> GetByIdAsync(int entityId);
        Task InsertAsync(Entity entity);
        Task RemoveAsync(int entityId);
        void Update(Entity entity);
        Task<int> MaximumOrderIndexAsync(int personId);
        Task<IEnumerable<Entity>> GetAllAsync();
    }
}
