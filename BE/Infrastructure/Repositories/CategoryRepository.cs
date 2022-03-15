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
        public override async Task<IEnumerable<Category>> GetAllAsync() =>
            await Context.Categories.AsNoTracking()
            .AsSplitQuery()
            .OrderBy(x => x.Name)
            .Include(x => x.Technologies)
            .ToListAsync();

        public override async Task<Category> GetByIdAsync(int id) =>
            await Context.Categories.Where(x => x.Id.Equals(id))
            .Include(x => x.Technologies)
            .SingleOrDefaultAsync();
        #endregion
    }
}
