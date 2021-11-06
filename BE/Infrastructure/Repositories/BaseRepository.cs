using Business.Domain.Repositories;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public abstract class BaseRepository<Entity> : IBaseRepository<Entity> where Entity : class
    {
        #region Property
        protected readonly AppDbContext Context;
        private DbSet<Entity> _entities;
        #endregion

        #region Constructor
        public BaseRepository(AppDbContext context)
        {
            this.Context = context;
            this._entities = Context.Set<Entity>();
        }
        #endregion

        #region Method
        public virtual async Task<Entity> GetByIdAsync(int entityId)
        {
            var statusName = Context.Model.FindEntityType(typeof(Entity)).GetProperty("Status").Name;
            var idName = Context.Model.FindEntityType(typeof(Entity)).GetProperty("Id").Name;

            return await _entities.Where(entity => EF.Property<int>(entity, idName).Equals(entityId) &&
                EF.Property<bool>(entity, statusName).Equals(true))
                .SingleOrDefaultAsync();
        }

        public virtual async Task InsertAsync(Entity entity)
            => await _entities.AddAsync(entity);

        /// <summary>
        /// Removing by change value of status true -> false
        /// </summary>
        /// <param name="entityId">Id of entity</param>
        public virtual async Task RemoveAsync(int entityId)
        {
            var entity = await GetByIdAsync(entityId);

            entity.GetType().GetProperty("Status").SetValue(entity, false);
        }

        public virtual void Update(Entity entity)
        => _entities.Update(entity);

        public virtual async Task<int> MaximumOrderIndexAsync(int personId)
        {
            var idName = Context.Model.FindEntityType(typeof(Entity)).GetProperty("PersonId").Name;

            var countElement = await _entities.Where(entity => EF.Property<int>(entity, idName).Equals(personId)).CountAsync();

            // Find maximum with OrderIndex predicate
            return countElement == 0 ? 1 : countElement + 1;
        }

        public virtual async Task<IEnumerable<Entity>> GetAllAsync()
        {
            var statusName = Context.Model.FindEntityType(typeof(Entity)).GetProperty("Status").Name;

            return await _entities.Where(entity => EF.Property<bool>(entity, statusName).Equals(true))
                .AsNoTracking()
                .ToListAsync();
        }
        #endregion
    }
}
