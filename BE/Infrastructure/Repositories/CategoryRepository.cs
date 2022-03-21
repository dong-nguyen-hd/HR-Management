using Business.Domain.Models;
using Business.Domain.Repositories;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        #region Constructor
        public CategoryRepository(AppDbContext context) : base(context) { }
        #endregion

        #region Method
        public override async Task<Category> GetByIdAsync(int id) =>
            await Context.Categories.Where(x => x.Id.Equals(id))
            .Include(x => x.Technologies)
            .SingleOrDefaultAsync();

        public async Task<IEnumerable<Category>> FindByNameAsync(string filterName)
        {
            var queryable = Context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(filterName))
                queryable = queryable.Where(x => x.Name.Contains(filterName));

            return await queryable
                .AsNoTracking()
                .AsSplitQuery()
                .OrderBy(x => x.Name)
                .Take(5)
                .Include(x => x.Technologies)
                .ToListAsync();
        }
        #endregion
    }
}
