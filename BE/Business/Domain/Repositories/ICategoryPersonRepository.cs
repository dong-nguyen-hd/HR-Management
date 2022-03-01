using Business.Domain.Models;
using System.Threading.Tasks;

namespace Business.Domain.Repositories
{
    public interface ICategoryPersonRepository : IBaseRepository<CategoryPerson>
    {
        Task<bool> ValidateExistent(int personId, int categoryId);
    }
}
