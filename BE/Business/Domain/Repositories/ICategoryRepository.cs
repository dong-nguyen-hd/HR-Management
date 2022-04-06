using Business.Domain.Models;
using Business.Resources;
using Business.Resources.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Domain.Repositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<IEnumerable<Category>> FindByNameAsync(string filterName, bool absolute = false);
        Task<(IEnumerable<Category> records, int total)> GetPaginationAsync(QueryResource pagination, FilterCategoryResource filterResource);
    }
}
