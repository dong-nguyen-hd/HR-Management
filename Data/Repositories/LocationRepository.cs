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
    public class LocationRepository : BaseRepository, ILocationRepository
    {
        public LocationRepository(AppDbContext context) : base(context) { }

        public async Task AddAsync(Location location)
            => await _context.Locations.AddAsync(location);

        public async Task<Location> FindByIdAsync(int id)
        {
            var temp = await _context.Locations
                .Where(x => x.Id == id && x.Status)
                .FirstOrDefaultAsync();

            return temp;
        }

        public async Task<IEnumerable<Location>> ListAsync()
        {
            var temp = await _context.Locations.Where(x => x.Status)
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Address)
                .AsNoTracking()
                .ToListAsync();

            return temp;
        }

        public void Remove(Location education)
            => _context.Locations.Remove(education);

        public void Update(Location location)
             => _context.Locations.Update(location);
    }
}
