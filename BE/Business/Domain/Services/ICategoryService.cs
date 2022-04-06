using Business.Communication;
using Business.Domain.Models;
using Business.Resources;
using Business.Resources.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Domain.Services
{
    public interface ICategoryService : IBaseService<CategoryResource, CreateCategoryResource, UpdateCategoryResource, Category>
    {
        Task<PaginationResponse<IEnumerable<CategoryResource>>> GetPaginationAsync(QueryResource pagination, FilterCategoryResource filterResource);
    }
}
