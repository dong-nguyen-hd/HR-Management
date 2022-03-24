using Business.Communication;
using Business.Domain.Models;
using Business.Resources;
using Business.Resources.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Domain.Services
{
    public interface IAccountService : IBaseService<AccountResource, CreateAccountResource, UpdateAccountResource, Account>
    {
        Task<PaginationResponse<IEnumerable<AccountResource>>> ListPaginationAsync(QueryResource pagintation);
        Task<BaseResponse<AccountResource>> SelfUpdateAsync(int id, SelfUpdateAccountResource resource);
        Task<BaseResponse<AccountResource>> UpdatePasswordAsync(int id, UpdatePasswordAccountResource resource);
    }
}
