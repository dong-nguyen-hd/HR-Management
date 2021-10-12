using Business.Domain.Models;
using Business.Domain.Repositories;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories
{
    public class LocationRepository : BaseRepository<Location>, ILocationRepository
    {
        #region Constructor
        public LocationRepository(AppDbContext context) : base(context) { }
        #endregion
    }
}
