using Business.Domain.Models;
using Business.Domain.Repositories;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TechnologyRepository : BaseRepository<Technology>, ITechnologyRepository
    {
        #region Constructor
        public TechnologyRepository(AppDbContext context) : base(context) { }
        #endregion

        #region Method
        public async Task<IEnumerable<Technology>> GetByCategoryAsync(int categoryId)
        => await Context.Technologies.Where(x => x.CategoryId == categoryId && x.Status)
            .OrderBy(x => x.Name)
            .AsNoTracking()
            .ToListAsync();

        public override async Task<IEnumerable<Technology>> GetAllAsync()
        => await Context.Technologies.Where(x => x.Status).OrderBy(x => x.Name).AsNoTracking().ToListAsync();
        #endregion
    }
}
