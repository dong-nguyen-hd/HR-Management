using HR_Management.Domain.Models;
using HR_Management.Resources.Authentication;
using HR_Management.Resources.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Domain.Repositories
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> ListPaginationAsync(QueryResource pagination);
        Task AddAsync(Account account);
        Task<int> TotalRecordAsync();
        void Update(Account account);
        Task<Account> FindByIdAsync(int id);
        Task<bool> ValidateUserNameAsync(string userName);
        Task<Account> ValidateCredentialsAsync(LoginResource loginResource);
        void Remove(Account account);
    }
}
