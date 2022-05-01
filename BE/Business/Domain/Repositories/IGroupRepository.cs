using Business.Domain.Models;

namespace Business.Domain.Repositories
{
    public interface IGroupRepository : IBaseRepository<Group>
    {
        Task<IEnumerable<Group>> FindByNameAsync(string filterName);
    }
}