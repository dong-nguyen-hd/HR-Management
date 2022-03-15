using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Extensions;
using Business.Resources;
using Business.Resources.Authentication;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class AccountRepository : BaseRepository<Account>, IAccountRepository
    {
        #region Constructor
        public AccountRepository(AppDbContext context) : base(context) { }
        #endregion

        #region Method
        public async Task<Account> GetByIdAsync(int id, bool hasToken)
        {
            var queryable = Context.Accounts.Where(x => x.Id.Equals(id));

            if (hasToken) queryable.Include(x => x.Tokens);

            return await queryable.SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Account>> ListPaginationAsync(QueryResource pagination) =>
            await Context.Accounts
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        public async Task<int> TotalRecordAsync() => await Context.Accounts.CountAsync();

        public async Task<Account> ValidateCredentialsAsync(LoginResource loginResource)
        {
            var accountStored = await Context.Accounts
                .Where(x => x.UserName == loginResource.UserName.ToLower())
                .SingleOrDefaultAsync();

            if (accountStored is null)
                return null;
            else
            {
                // Validate credential
                bool isValid = accountStored.Password.CheckingPassword(loginResource.Password);
                if (isValid)
                    return accountStored;
                else
                    return null;
            }
        }

        /// <summary>
        /// Is user-name contain in database
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>
        /// <list type="bullet">
        /// <item>True: valid</item>
        /// <item>False: invalid</item>
        /// </list>
        /// </returns>
        public async Task<bool> ValidateUserNameAsync(string userName) =>
            !await Context.Accounts.Where(x => x.UserName == userName).AnyAsync();
        #endregion
    }
}
