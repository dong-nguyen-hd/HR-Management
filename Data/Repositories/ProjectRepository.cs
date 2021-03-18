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
    public class ProjectRepository : BaseRepository, IProjectRepository
    {
        public ProjectRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Project>> ListAsync(int personId)
        {
            var temp = await _context.Projects.Where(x => x.PersonId == personId && x.Status)
                .OrderBy(x => x.OrderIndex)
                .ThenBy(x => x.EndDate)
                .ThenBy(x => x.StartDate)
                .ThenBy(x => x.Name)
                .AsNoTracking()
                .ToListAsync();

            return temp;
        }

        public async Task AddAsync(Project project)
            => await _context.Projects.AddAsync(project);

        public void Update(Project project)
            => _context.Projects.Update(project);

        public async Task<Project> FindByIdAsync(int id)
        {
            var temp = await _context.Projects
                .Where(x => x.Id == id && x.Status)
                .FirstOrDefaultAsync();

            return temp;
        }

        public void Remove(Project project)
            => _context.Projects.Remove(project);

        public int MaximumOrderIndex(int personId)
        {
            var tempList = _context.Projects
                .Where(x => x.PersonId == personId && x.Status)
                .Select(x => new { x.OrderIndex })
                .AsNoTracking();

            int maximum = (tempList.Count() == 0) ? 0 : tempList.Max(x => x.OrderIndex);

            return maximum;
        }
    }
}
