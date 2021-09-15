#nullable enable
using HR_Management.Data.Contexts;
using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using HR_Management.Extensions;
using HR_Management.Resources.Authentication;
using HR_Management.Resources.Queries;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Management.Data.Repositories
{
    public class AccountRepository : BaseRepository, IAccountRepository
    {
        public AccountRepository(AppDbContext context) : base(context) { }

        public async Task AddAsync(Account account)
            => await _context.Accounts.AddAsync(account);

        public async Task<Account> FindByIdAsync(int id)
        {
            var temp = await _context.Accounts
                .Where(x => x.Id == id && x.Status)
                .FirstOrDefaultAsync();

            return temp;
        }

        public async Task<IEnumerable<Account>> ListPaginationAsync(QueryResource pagination)
        {
            var temp = await _context.Accounts
                .Where(x => x.Status)
                .OrderBy(x => x.Id)
                .Skip((pagination.Page - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .AsNoTracking()
                .ToListAsync();

            return temp;
        }

        public void Remove(Account account)
            => _context.Accounts.Remove(account);

        public async Task<int> TotalRecordAsync()
            => await _context.Accounts.CountAsync();

        public void Update(Account account)
            => _context.Accounts.Update(account);

        public async Task<Account> ValidateCredentialsAsync(LoginResource loginResource)
        {
            var temp = await _context.Accounts
                .Where(x => x.UserName == loginResource.UserName.ToLower())
                .FirstOrDefaultAsync();

            if (temp is null)
                return null;
            else
            {
                // Validate credential
                bool isValid = temp.Password.CheckingPassword(loginResource.Password);
                if (isValid)
                    return temp;
                else
                    return null;
            }
        }

        public async Task<bool> ValidateUserNameAsync(string userName)
           => !await _context.Accounts.Where(x => x.UserName == userName).AnyAsync();
    }
}
