using Business.Domain.Models;
using Business.Resources;
using Business.Resources.Authentication;

namespace Business.Domain.Repositories
{
    public interface IAccountRepository : IBaseRepository<Account>
    {
        Task<IEnumerable<Account>> ListPaginationAsync(QueryResource pagination);
        Task<int> TotalRecordAsync();
        Task<bool> ValidateUserNameAsync(string userName);
        Task<Account> ValidateCredentialsAsync(LoginResource loginResource);
        Task<Account> GetByIdAsync(int id, bool hasToken);
    }
}
