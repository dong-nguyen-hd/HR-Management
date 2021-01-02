#nullable enable
using HR_Management.Data.Contexts;
using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Management.Data.Repositories
{
    public class WorkHistoryRepository : BaseRepository, IWorkHistoryRepository
    {
        public WorkHistoryRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<WorkHistory>> ListAsync(int personId)
        {
            var temp = await _context.WorkHistories.Where(x => x.PersonId == personId && x.Status)
                .OrderBy(x => x.OrderIndex)
                .ThenBy(x => x.EndDate)
                .ToListAsync();

            return temp;
        }

        public async Task AddAsync(WorkHistory workHistory)
        {
            await _context.WorkHistories.AddAsync(workHistory);
        }

        public void Update(WorkHistory workHistory)
        {
            _context.WorkHistories.Update(workHistory);
        }

        public async Task<WorkHistory> FindByIdAsync(int id)
        {
            var temp = await _context.WorkHistories.Where(x => x.Id == id && x.Status).FirstOrDefaultAsync();

            return temp;
        }

        public void Remove(WorkHistory workHistory)
        {
            _context.WorkHistories.Remove(workHistory);
        }

        public async Task<int> MaximumOrderIndexAsync(int personId)
        {
            var tempList = await _context.WorkHistories.Where(x => x.PersonId == personId && x.Status).ToListAsync();
            int maximum = (tempList.Count == 0) ? 0 : tempList.Max(x => x.OrderIndex);

            return maximum;
        }
    }
}
