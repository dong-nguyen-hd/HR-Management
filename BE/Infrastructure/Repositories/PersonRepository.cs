﻿using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Resources;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class PersonRepository : BaseRepository<Person>, IPersonRepository
    {
        #region Constructor
        public PersonRepository(AppDbContext context) : base(context) { }
        #endregion

        #region Method
        public async Task<IEnumerable<Person>> GetPaginationAsync(QueryResource pagination, int? locationId = null)
        {
            var queryable = Context.People.OrderBy(x => x.Id).Where(x => x.Status);

            if (locationId != null)
                queryable.Where(x => x.LocationId == locationId);

            return await queryable.Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Include(y => y.Location)
                .Include(y => y.WorkHistories.Where(z => z.Status))
                .Include(y => y.CategoryPersons.Where(z => z.Status))
                .Include(y => y.Educations.Where(z => z.Status))
                .Include(y => y.Certificates.Where(z => z.Status))
                .Include(y => y.Projects.Where(z => z.Status))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Person> GetByIdAsync(string staffId)
            => await Context.People
                .Include(y => y.Location)
                .Include(y => y.WorkHistories.Where(z => z.Status))
                .Include(y => y.CategoryPersons.Where(z => z.Status))
                .Include(y => y.Educations.Where(z => z.Status))
                .Include(y => y.Certificates.Where(z => z.Status))
                .Include(y => y.Projects.Where(z => z.Status))
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Status && x.StaffId == staffId);

        public async Task<int> TotalRecordAsync()
            => await Context.People.CountAsync(x => x.Status);
        #endregion
    }
}
