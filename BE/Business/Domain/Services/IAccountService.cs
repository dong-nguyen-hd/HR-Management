using Business.Communication;
using Business.Domain.Models;
using Business.Resources;
using Business.Resources.Account;

namespace Business.Domain.Services
{
    public interface IAccountService : IBaseService<AccountResource, CreateAccountResource, UpdateAccountResource, Account>
    {
        Task<PaginationResponse<IEnumerable<AccountResource>>> GetPaginationAsync(QueryResource pagintation, FilterAccountResource filterResource);
        Task<BaseResponse<AccountResource>> SelfUpdateAsync(int id, SelfUpdateAccountResource resource);
        Task<BaseResponse<AccountResource>> UpdatePasswordAsync(int id, UpdatePasswordAccountResource resource);
    }
}
