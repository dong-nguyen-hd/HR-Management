using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Extensions;
using Business.Resources;
using Business.Resources.Person;
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
        public async Task<(IEnumerable<Person> records, int total)> GetPaginationAsync(QueryResource pagination, FilterPersonResource filterResource)
        {
            var queryable = Context.People.Where(x => x.Status);

            if (filterResource != null)
            {
                if (!string.IsNullOrEmpty(filterResource.StaffId))
                    queryable = queryable.Where(x => x.StaffId.Equals(filterResource.StaffId.RemoveSpaceCharacter()));

                if (filterResource.LocationId != null)
                    queryable = queryable.Where(x => x.LocationId.Equals(filterResource.LocationId));

                if (!string.IsNullOrEmpty(filterResource.FirstName))
                {
                    string fullName = filterResource.FirstName.RemoveSpaceCharacter().ToLower();
                    queryable = queryable.Where(x => x.FirstName.Contains(fullName));
                }
            }

            var total = await queryable.CountAsync();

            var records = await queryable.OrderBy(x => x.Id).Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Include(y => y.Location)
                .Include(y => y.WorkHistories.Where(z => z.Status))
                .Include(y => y.CategoryPersons.Where(z => z.Status))
                .ThenInclude(z => z.Category)
                .Include(y => y.Educations.Where(z => z.Status))
                .Include(y => y.Certificates.Where(z => z.Status))
                .Include(y => y.Projects.Where(z => z.Status))
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();

            return (records, total);
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
