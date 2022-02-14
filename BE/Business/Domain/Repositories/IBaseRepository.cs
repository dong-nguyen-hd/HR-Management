using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Business.Domain.Repositories
{
    public interface IBaseRepository<Entity> where Entity : class
    {
        Task<Entity> GetByIdAsync(int entityId);
        Task<IEnumerable<Entity>> GetWithPrimaryKeyAsync(List<int> keyValues);
        Task InsertAsync(Entity entity);
        Task RemoveAsync(int entityId);
        Task<int> RemoveRangeAsync(IEnumerable<Entity> entities);
        void Update(Entity entity);
        Task<int> MaximumOrderIndexAsync(int personId);
        Task<IEnumerable<Entity>> GetAllAsync();
        Task<IEnumerable<Entity>> FindAsync(Expression<Func<Entity, bool>> expression);
    }
}
