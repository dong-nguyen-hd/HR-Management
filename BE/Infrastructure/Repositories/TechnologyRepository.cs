using Business.Domain.Models;
using Business.Domain.Repositories;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories
{
    public class TechnologyRepository : BaseRepository<Technology>, ITechnologyRepository
    {
        #region Constructor
        public TechnologyRepository(AppDbContext context) : base(context) { }
        #endregion
    }
}