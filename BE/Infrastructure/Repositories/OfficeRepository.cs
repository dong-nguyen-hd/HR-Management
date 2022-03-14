using Business.Domain.Models;
using Business.Domain.Repositories;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories
{
    public class OfficeRepository : BaseRepository<Office>, IOfficeRepository
    {
        #region Constructor
        public OfficeRepository(AppDbContext context) : base(context) { }
        #endregion
    }
}
