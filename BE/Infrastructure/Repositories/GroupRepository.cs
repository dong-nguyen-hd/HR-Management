using Business.Domain.Models;
using Business.Domain.Repositories;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        #endregion
    }
}
