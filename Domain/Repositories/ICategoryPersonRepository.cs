using HR_Management.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Domain.Repositories
{
    public interface ICategoryPersonRepository
    {
        Task<IEnumerable<CategoryPerson>> ListAsync(int personId);
        Task AddAsync(CategoryPerson categoryPerson);
        void Update(CategoryPerson categoryPerson);
        Task<CategoryPerson> FindByIdAsync(int id);
        void Remove(CategoryPerson categoryPerson);
        int MaximumOrderIndex(int personId);
    }
}
