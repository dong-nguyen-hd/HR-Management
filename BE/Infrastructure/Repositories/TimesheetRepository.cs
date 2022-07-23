using Business.Domain.Models;
using Business.Domain.Repositories;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TimesheetRepository : BaseRepository<Timesheet>, ITimesheetRepository
    {
        #region Constructor
        public TimesheetRepository(AppDbContext context) : base(context) { }
        #endregion

        #region Method
        public async Task<Timesheet> GetTimesheetByPersonIdAsync(int personId) =>
            await Context.Timesheets.SingleOrDefaultAsync(x => x.PersonId == personId);
        #endregion
    }
}
