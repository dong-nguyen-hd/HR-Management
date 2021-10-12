using Business.Domain.Models;
using Business.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Domain.Repositories
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        Task<IEnumerable<Person>> GetPaginationAsync(QueryResource pagination, int? locationId = null);
        Task<Person> GetByIdAsync(string staffId);
        Task<int> TotalRecordAsync();
    }
}
