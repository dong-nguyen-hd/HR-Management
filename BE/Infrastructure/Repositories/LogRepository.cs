using Business.Domain.Models;
using Business.Domain.Repositories;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories
{
    public class LogRepository : BaseRepository<Log>, ILogRepository
    {
        #region Constructor
        public LogRepository(AppDbContext context) : base(context) { }
        #endregion
    }
}
