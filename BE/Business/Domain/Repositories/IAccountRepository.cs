using Business.Domain.Models;
using Business.Resources;
using Business.Resources.Authentication;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Domain.Repositories
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        Task<IEnumerable<Account>> ListPaginationAsync(QueryResource pagination);
        Task<int> TotalRecordAsync();
        Task<bool> ValidateUserNameAsync(string userName);
        Task<Account> ValidateCredentialsAsync(LoginResource loginResource);
    }
}
