using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Extensions;
using Business.Resources;
using Business.Resources.Group;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        #region Constructor
        public GroupRepository(AppDbContext context) : base(context) { }
        #endregion

        #region Method
        public async Task<IEnumerable<Group>> FindByNameAsync(string filterName)
        {
            var queryable = Context.Groups.AsQueryable();

            if (!string.IsNullOrEmpty(filterName))
                queryable = queryable.Where(x => x.Name.Contains(filterName));

            return await queryable
                .AsNoTracking()
                .OrderByDescending(x => x.StartDate)
                .Take(5)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Group> records, int total)> GetPaginationAsync(QueryResource pagination, FilterGroupResource filterResource)
        {
            var queryable = ConditionFilter(filterResource);

            var total = await queryable.CountAsync();

            var records = await queryable.AsNoTracking()
                .OrderByDescending(x => x.Name)
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return (records, total);
        }

        private IQueryable<Group> ConditionFilter(FilterGroupResource filterResource)
        {
            var queryable = Context.Groups.AsQueryable();

            if (filterResource != null)
            {
                if (!string.IsNullOrEmpty(filterResource.Name))
                {
                    string name = filterResource.Name.RemoveSpaceCharacter().ToLower();
                    queryable = queryable.Where(x => x.Name.Contains(name));
                }

                if (filterResource.Available)
                    queryable = queryable.Where(x => x.EndDate == null);
            }

            return queryable;
        }
        #endregion
    }
}
