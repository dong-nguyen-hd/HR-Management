using HR_Management.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Domain.Repositories
{
    public interface IPersonRepository
    {
        Task<IEnumerable<Person>> ListAsync(int id);
        Task AddAsync(Person person);
        void Update(Person person);
        Task<Person> FindByIdAsync(int id);
        void Remove(Person person);
    }
}
