using Business.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Domain.Repositories
{
    public interface ITechnologyRepository : IBaseRepository<Technology>
    {
        Task<IEnumerable<Technology>> FindByNameAsync(string filterName);
    }
}
