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
    public class CategoryPersonRepository : BaseRepository, ICategoryPersonRepository
    {
        public CategoryPersonRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<CategoryPerson>> ListAsync(int personId)
        {
            var temp = await _context.CategoryPersons.Where(x => x.PersonId == personId && x.Status)
                .OrderBy(x => x.OrderIndex)
                .AsNoTracking()
                .ToListAsync();

            return temp;
        }

        public async Task AddAsync(CategoryPerson categoryPerson)
            => await _context.CategoryPersons.AddAsync(categoryPerson);

        public void Update(CategoryPerson categoryPerson)
            => _context.CategoryPersons.Update(categoryPerson);

        public async Task<CategoryPerson> FindByIdAsync(int id)
        {
            var temp = await _context.CategoryPersons.Where(x => x.Id == id && x.Status).FirstOrDefaultAsync();

            return temp;
        }

        public void Remove(CategoryPerson categoryPerson)
            => _context.CategoryPersons.Remove(categoryPerson);

        public int MaximumOrderIndex(int personId)
        {
            var tempList = _context.CategoryPersons
                .Where(x => x.PersonId == personId && x.Status)
                .Select(x => new {x.OrderIndex})
                .AsNoTracking();

            int maximum = (tempList.Count() == 0) ? 0 : tempList.Max(x => x.OrderIndex);

            return maximum;
        }
    }
}
