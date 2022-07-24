using Business.Domain.Models;

namespace Business.Domain.Repositories
{
    public interface ITimesheetRepository : IBaseRepository<Timesheet>
    {
        Task<Timesheet> GetTimesheetByPersonIdAsync(int personId, DateTime date);
    }
}
