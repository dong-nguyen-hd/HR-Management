using HR_Management.Domain.Services.Communication;
using HR_Management.Resources.Account;
using HR_Management.Resources.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface IAccountService
    {
        Task<AccountResponse<IEnumerable<AccountResource>>> ListWithPaginationAsync(QueryResource pagintation, string route);
        Task<AccountResponse<AccountResource>> FindByIdAsync(int id);
        Task<AccountResponse<AccountResource>> CreateAsync(CreateAccountResource createAccountResource);
        Task<AccountResponse<AccountResource>> UpdateAsync(int id, UpdateAccountResource updateAccountResource);
        Task<AccountResponse<AccountResource>> DeleteAsync(int id);
    }
}
