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
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Category>> ListAsync()
        {
            var temp = await _context.Categories.Where(x => x.Status)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return temp;
        }

        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }

        public async Task<Category> FindByIdAsync(int id)
        {
            var temp = await _context.Categories.Where(x => x.Id == id && x.Status).FirstOrDefaultAsync();

            return temp;
        }

        public void Remove(Category category)
        {
            _context.Categories.Remove(category);
        }
    }
}
