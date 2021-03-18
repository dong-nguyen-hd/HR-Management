#nullable enable
using HR_Management.Data.Contexts;
using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Management.Data.Repositories
{
    public class TechnologyRepository : BaseRepository, ITechnologyRepository
    {
        public TechnologyRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Technology>> ListAsync()
        {
            var temp = await _context.Technologies
                .Where(x => x.Status)
                .OrderBy(x => x.Name)
                .AsNoTracking()
                .ToListAsync();

            return temp;
        }

        public async Task<IEnumerable<Technology>> ListAsync(int categoryId)
        {
            var temp = await _context.Technologies
                .Where(x => x.CategoryId == categoryId && x.Status)
                .OrderBy(x => x.Name)
                .AsNoTracking()
                .ToListAsync();

            return temp;
        }

        public async Task AddAsync(Technology technology)
            => await _context.Technologies.AddAsync(technology);

        public void Update(Technology technology)
            => _context.Technologies.Update(technology);

        public async Task<Technology> FindByIdAsync(int id)
        {
            var temp = await _context.Technologies
                .Where(x => x.Id == id && x.Status)
                .FirstOrDefaultAsync();

            return temp;
        }

        public void Remove(Technology technology)
            => _context.Technologies.Remove(technology);
    }
}
