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
        public async Task<IEnumerable<Technology>> FindByNameAsync(string filterName)
        {
            var queryable = Context.Technologies.Where(x => x.Status);

            if (!string.IsNullOrEmpty(filterName))
                queryable = queryable.Where(x => x.Name.Contains(filterName));

            return await queryable.OrderBy(x => x.Name).Take(5).AsNoTracking().ToListAsync();
        }
        #endregion
    }
}