using HR_Management.Domain.Models;
using HR_Management.Resources.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Domain.Repositories
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> ListPaginationAsync(QueryResource pagintation);
        Task<IEnumerable<Person>> ListByLocationAsync(QueryResource pagination, int locationId);
        Task<int> TotalRecordAsync();
        Task AddAsync(Person person);
        void Update(Person person);
        Task<Person> FindByIdAsync(int id);
        void Remove(Person person);
    }
}
