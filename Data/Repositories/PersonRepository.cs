#nullable enable
using HR_Management.Data.Contexts;
using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using HR_Management.Resources.Queries;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Management.Data.Repositories
{
    public class PersonRepository : BaseRepository, IPersonRepository
    {
        public PersonRepository(AppDbContext context) : base(context) { }

        public async Task AddAsync(Person person)
            => await _context.People.AddAsync(person);

        public async Task<Person> FindByIdAsync(int id)
        {
            var temp = await _context.People.FirstOrDefaultAsync(x => x.Id == id && x.Status);

            return temp;
        }
        public async Task<IEnumerable<Person>> ListPaginationAsync(QueryResource pagination)
        {
            var temp = await _context.People
                .Where(x => x.Status)
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Include(y => y.Location)
                .Include(y => y.WorkHistories)
                .Include(y => y.CategoryPersons)
                .Include(y => y.Educations)
                .Include(y => y.Certificates)
                .Include(y => y.Projects)
                .AsNoTracking()
                .ToListAsync();

            return temp;
        }

        public async Task<IEnumerable<Person>> ListByLocationAsync(QueryResource pagination, int locationId)
        {
            var temp = await _context.People.Where(x => x.LocationId == locationId && x.Status)
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.StaffId)
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Include(y => y.Location)
                .AsNoTracking()
                .ToListAsync();

            return temp;
        }

        public void Remove(Person person)
            => _context.People.Remove(person);

        public void Update(Person person)
            => _context.People.Update(person);

        public async Task<int> TotalRecordAsync()
            => await _context.People.CountAsync();
    }
}
