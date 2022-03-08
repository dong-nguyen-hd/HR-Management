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
            await Context.Categories.Where(x => x.Status.Equals(true)).Include(x => x.Technologies)
            .OrderBy(x => x.Name)
            .AsNoTracking()
            .AsSplitQuery()
            .ToListAsync();

        public override async Task<Category> GetByIdAsync(int id) =>
            await Context.Categories.Where(x => x.Status.Equals(true) && x.Id.Equals(id))
            .Include(x => x.Technologies)
            .SingleOrDefaultAsync();
        #endregion
    }
}
