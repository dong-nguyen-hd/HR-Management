using HR_Management.Data.Contexts;
using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Management.Data.Repositories
{
    public class EducationRepository : BaseRepository, IEducationRespository
    {
        public EducationRepository(AppDbContext context) : base(context){ }

        public async Task<IEnumerable<Education>> ListAsync()
        {
            return await _context.Educations.ToListAsync();
        }
    }
}
