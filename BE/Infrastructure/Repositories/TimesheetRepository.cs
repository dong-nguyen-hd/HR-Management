using Business.Domain.Models;
using Business.Domain.Repositories;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories
{
    public class TimesheetRepository : BaseRepository<Timesheet>, ITimesheetRepository
    {
        #region Constructor
        public TimesheetRepository(AppDbContext context) : base(context) { }
        #endregion

        #region Method
        
        #endregion
    }
}
