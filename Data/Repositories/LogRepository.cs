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
    public class LogRepository : BaseRepository, ILogRepository
    {
        public LogRepository(AppDbContext context) : base(context) { }

        public async Task AddAsync(Log log)
            => await _context.Logs.AddAsync(log);

        /// <summary>
        /// Getting top 5th of list
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Log>> ListAsync(int personId)
        {
            var temp = await _context.Logs
                .Where(x => x.Id == personId)
                .OrderByDescending(x => x.UpdatedAt)
                .Take(5)
                .AsNoTracking()
                .ToListAsync();

            return temp;
        }
    }
}
