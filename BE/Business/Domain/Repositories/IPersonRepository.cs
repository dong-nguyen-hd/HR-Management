using Business.Domain.Models;
using Business.Resources;
using Business.Resources.Person;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Domain.Repositories
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        Task<(IEnumerable<Person> records, int total)> GetPaginationAsync(QueryResource pagination, FilterPersonResource filterResource);
        Task<Person> GetByIdAsync(string staffId);
        Task<int> TotalRecordAsync();
    }
}
