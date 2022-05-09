using Business.Domain.Models;

namespace Business.Domain.Repositories
{
    public interface IOfficeRepository : IBaseRepository<Office>
    {
        public Task<List<Office>> FindByNameAsync(string filterName, bool absolute = false);
    }
}
