using Business.Communication;
using Business.Domain.Models;
using Business.Resources.Technology;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Domain.Services
{
    public interface ITechnologyService : IBaseService<TechnologyResource, CreateTechnologyResource, UpdateTechnologyResource, Technology>
    {
        Task<BaseResponse<IEnumerable<TechnologyResource>>> GetByCategoryAsync(int categoryId);
    }
}
