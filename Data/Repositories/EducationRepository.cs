using HR_Management.Data.Contexts;
using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Management.Data.Repositories
{
    public class EducationRepository : BaseRepository, IEducationRespository
    {
        public EducationRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Education>> ListAsync(int personId)
        {
            var temp =  await _context.Educations.Where(x => x.PersonId == personId).ToListAsync();

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
    }
}
