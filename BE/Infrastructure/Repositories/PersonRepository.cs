using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Extensions;
using Business.Resources;
using Business.Resources.Person;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
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
            var queryable = ConditionFilter(filterResource);

            var total = await queryable.CountAsync();

            var records = await queryable.OrderBy(x => x.Id).Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .Include(y => y.Office)
                .Include(y => y.WorkHistories.Where(z => z.Status))
                .Include(y => y.CategoryPersons.Where(z => z.Status))
                .ThenInclude(z => z.Category)
                .Include(y => y.Educations.Where(z => z.Status))
                .Include(y => y.Certificates.Where(z => z.Status))
                .Include(y => y.Projects.Where(z => z.Status))
                .ThenInclude(z => z.Group)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();

            return (records, total);
        }

        private IQueryable<Person> ConditionFilter(FilterPersonResource filterResource)
        {
            var queryable = Context.People.Where(x => x.Status);

            if (filterResource != null)
            {
                if (!string.IsNullOrEmpty(filterResource.StaffId))
                    queryable = queryable.Where(x => x.StaffId.Equals(filterResource.StaffId.RemoveSpaceCharacter()));

                if (filterResource.OfficeId != null)
                    queryable = queryable.Where(x => x.OfficeId.Equals(filterResource.OfficeId));

                if (filterResource.Available)
                    queryable = queryable.Where(x => x.GroupId.Equals(null));

                if (!string.IsNullOrEmpty(filterResource.FirstName))
                {
                    string fullName = filterResource.FirstName.RemoveSpaceCharacter().ToLower();
                    queryable = queryable.Where(x => x.FirstName.Contains(fullName));
                }

                if (filterResource.TechnologyId?.Count > 0)
                {
                    var tempQueryable = Context.CategoryPersons.Where(x => x.Status);

                    int count = filterResource.TechnologyId.Count;

                    // I only use 3 conditions, but you can customize conditions with more options
                    switch (count)
                    {
                        case 2:
                            {
                                var listPersonIdOne = tempQueryable.Where(x => x.Technology.Contains(filterResource.TechnologyId[0].ToString())).Select(x => x.PersonId);

                                tempQueryable = tempQueryable.Where(x => listPersonIdOne.Contains(x.PersonId) &&
                                x.Technology.Contains(filterResource.TechnologyId[1].ToString()));
                                break;
                            }
                        case 3:
                            {
                                var listPersonIdOne = tempQueryable.Where(x => x.Technology.Contains(filterResource.TechnologyId[0].ToString())).Select(x => x.PersonId);

                                var listPersonIdTwo = tempQueryable.Where(x => listPersonIdOne.Contains(x.PersonId) &&
                                x.Technology.Contains(filterResource.TechnologyId[1].ToString())).Select(x => x.PersonId);

                                tempQueryable = tempQueryable.Where(x => listPersonIdTwo.Contains(x.PersonId) &&
                                x.Technology.Contains(filterResource.TechnologyId[2].ToString()));
                                break;
                            }
                        default:
                            tempQueryable = tempQueryable.Where(x => x.Technology.Contains(filterResource.TechnologyId[0].ToString()));
                            break;
                    }

                    var listId = tempQueryable.Select(y => y.PersonId);

                    queryable = queryable.Where(x => listId.Contains(x.Id));
                    // Console.WriteLine($"SQl Query: {queryable.ToQueryString()}"); // Debug performance of sql query
                }
            }

            return queryable;
        }

        public override async Task<Person> GetByIdAsync(int id) =>
            await Context.People
                .Include(y => y.Office)
                .Include(y => y.WorkHistories.Where(z => z.Status))
                .Include(y => y.CategoryPersons.Where(z => z.Status))
                .ThenInclude(z => z.Category)
                .Include(y => y.Educations.Where(z => z.Status))
                .Include(y => y.Certificates.Where(z => z.Status))
                .Include(y => y.Projects.Where(z => z.Status))
                .ThenInclude(z => z.Group)
                .AsNoTracking()
                .AsSplitQuery()
                .SingleOrDefaultAsync(x => x.Status && x.Id == id);

        public async Task<int> TotalRecordAsync() =>
            await Context.People.CountAsync(x => x.Status);
        #endregion
    }
}
