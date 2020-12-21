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
    public class EducationRepository : BaseRepository, IEducationRepository
    {
        public EducationRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Education>> ListAsync(int personId)
        {
            var temp =  await _context.Educations.Where(x => x.PersonId == personId && x.Status)
                .OrderBy(x => x.OrderIndex)
                .ToListAsync();
            
            return temp;
        }

        public async Task AddAsync(Education education)
        {
            await _context.Educations.AddAsync(education);
        }

        public void Update(Education education)
        {
            _context.Educations.Update(education);
        }

        public async Task<Education> FindByIdAsync(int id)
        {
            var temp = await _context.Educations.Where(x => x.Id == id && x.Status).FirstOrDefaultAsync();

            return temp;
        }

        public void Remove(Education education)
        {
            _context.Educations.Remove(education);
        }

        public async Task<int> MaximumOrderIndexAsync(int personId)
        {
            var maximum = await _context.Educations.Where(x => x.PersonId == personId && x.Status).MaxAsync(y => y.OrderIndex);

            return maximum;
        }
    }
}
