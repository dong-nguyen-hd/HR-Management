using Business.Domain.Models;
using Business.Domain.Repositories;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CategoryPersonRepository : BaseRepository<CategoryPerson>, ICategoryPersonRepository
    {
        #region Constructor
        public CategoryPersonRepository(AppDbContext context) : base(context) { }
        #endregion

        #region Method
        public async Task<bool> ValidateExistent(int personId, int categoryId) =>
            await Context.CategoryPersons
            .Where(x => x.PersonId.Equals(personId) && x.Id.Equals(categoryId))
            .SingleOrDefaultAsync() is null ? false : true;
        #endregion
    }
}
