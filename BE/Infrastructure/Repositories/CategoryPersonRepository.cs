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
            .Where(x => x.PersonId.Equals(personId) && x.CategoryId.Equals(categoryId))
            .SingleOrDefaultAsync() is null ? false : true;

        public async override Task<CategoryPerson> GetByIdAsync(int id) =>
            await Context.CategoryPersons.Where(x => x.Id.Equals(id))
            .Include(x => x.Category)
            .ThenInclude(y => y.Technologies)
            .SingleOrDefaultAsync();
        #endregion
    }
}
