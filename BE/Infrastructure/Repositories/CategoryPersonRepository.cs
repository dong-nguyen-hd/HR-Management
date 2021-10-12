using Business.Domain.Models;
using Business.Domain.Repositories;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories
{
    public class CategoryPersonRepository : BaseRepository<CategoryPerson>, ICategoryPersonRepository
    {
        #region Constructor
        public CategoryPersonRepository(AppDbContext context) : base(context) { }
        #endregion
    }
}
