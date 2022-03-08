using Business.Domain.Repositories;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
        public virtual async Task<IEnumerable<Entity>> FindAsync(Expression<Func<Entity, bool>> expression) =>
            await _entities.Where(expression).ToListAsync();

        public virtual async Task<Entity> GetByIdAsync(int entityId)
        {
            var statusName = Context.Model.FindEntityType(typeof(Entity)).GetProperty("Status").Name;
            var idName = Context.Model.FindEntityType(typeof(Entity)).GetProperty("Id").Name;

            return await _entities.Where(entity => EF.Property<int>(entity, idName).Equals(entityId) &&
                EF.Property<bool>(entity, statusName).Equals(true))
                .SingleOrDefaultAsync();
        }

        public virtual async Task InsertAsync(Entity entity) =>
            await _entities.AddAsync(entity);

        /// <summary>
        /// Removing by change value of status true -> false
        /// </summary>
        /// <param name="entityId">Id of entity</param>
        public virtual async Task RemoveAsync(int entityId)
        {
            var entity = await GetByIdAsync(entityId);

            entity.GetType().GetProperty("Status").SetValue(entity, false);
        }

        public virtual void Update(Entity entity) =>
            _entities.Update(entity);

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
            var propertyName = Context.Model.FindEntityType(typeof(Entity)).GetProperty("Name").Name;

            return await _entities.Where(entity => EF.Property<bool>(entity, statusName).Equals(true))
                .OrderBy(entity => EF.Property<string>(entity, propertyName)) // Warning: make sure the Entity contain "Name" property!
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Removing by change value of status true -> false
        /// </summary>
        /// <param name="entities"></param>
        /// <returns>The number of entities deleted</returns>
        public virtual int RemoveRange(IEnumerable<Entity> entities)
        {
            int count = 0;
            foreach (var entity in entities)
            {
                entity.GetType().GetProperty("Status").SetValue(entity, false);
                count++;
            }

            return count;
        }

        /// <summary>
        /// Get entity from table with many primary key
        /// </summary>
        /// <param name="keyValues">You can use "params object[] keyValues" for generic type</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Only a single primary key is supported</exception>
        /// <exception cref="ArgumentException">Type does not contain the primary key as an accessible property</exception>
        public virtual async Task<IEnumerable<Entity>> GetWithPrimaryKeyAsync(List<int> keyValues)
        {
            var entityType = Context.Model.FindEntityType(typeof(Entity));
            var primaryKey = entityType.FindPrimaryKey();

            if (primaryKey.Properties.Count != 1)
                throw new NotSupportedException("Only a single primary key is supported");

            var pkProperty = primaryKey.Properties[0];
            var pkPropertyType = pkProperty.ClrType;

            // Validate passed key values
            foreach (var keyValue in keyValues)
            {
                if (!pkPropertyType.IsAssignableFrom(keyValue.GetType()))
                    throw new ArgumentException($"Key value '{keyValue}' is not of the right type");
            }

            // Retrieve member info for primary key
            var pkMemberInfo = typeof(Entity).GetProperty(pkProperty.Name);
            if (pkMemberInfo is null)
                throw new ArgumentException("Type does not contain the primary key as an accessible property");

            // Build lambda expression
            var parameter = Expression.Parameter(typeof(Entity), "e");
            var body = Expression.Call(null, ContainsMethod,
                Expression.Constant(keyValues),
                Expression.Convert(Expression.MakeMemberAccess(parameter, pkMemberInfo), typeof(int)));

            var predicateExpression = Expression.Lambda<Func<Entity, bool>>(body, parameter);

            return await _entities.Where(predicateExpression).ToListAsync();
        }

        private static MethodInfo ContainsMethod =>
            typeof(Enumerable).GetMethods()
            .FirstOrDefault(m => m.Name == "Contains" && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(int));
        #endregion
    }
}
