using Business.Communication;
using Business.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Domain.Services
{
    public interface IBaseService<Response, Insert, Update, Entity>
    {
        Task<BaseResponse<Response>> GetByIdAsync(int id);
        Task<BaseResponse<IEnumerable<Response>>> GetAllAsync();
        Task<BaseResponse<Response>> InsertAsync(Insert insertResource);
        Task<BaseResponse<Response>> UpdateAsync(int id, Update updateResource);
        Task<BaseResponse<Response>> RemoveAsync(int id);
        Task<BaseResponse<Response>> SwapAsync(SwapResource swapResource);
    }
}
