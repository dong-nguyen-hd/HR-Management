using Business.Domain.Models;
using Business.Domain.Repositories;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class OfficeRepository : BaseRepository<Office>, IOfficeRepository
    {
        #region Constructor
        public OfficeRepository(AppDbContext context) : base(context) { }
        #endregion

        #region Method
        public async Task<List<Office>> FindByNameAsync(string filterName, bool absolute = false)
        {
            var queryable = Context.Offices.AsQueryable();

            if (!string.IsNullOrEmpty(filterName) && !absolute)
                queryable = queryable.Where(x => x.Name.Contains(filterName));

            if (!string.IsNullOrEmpty(filterName) && absolute)
                queryable = queryable.Where(x => x.Name.Equals(filterName));

            return await queryable
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .Take(5)
                .ToListAsync();
        }
        #endregion
    }
}
