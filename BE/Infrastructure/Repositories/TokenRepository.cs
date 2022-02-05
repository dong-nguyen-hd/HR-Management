using Business.Domain.Models;
using Business.Domain.Repositories;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TokenRepository : BaseRepository<Token>, ITokenRepository
    {
        #region Constructor
        public TokenRepository(AppDbContext context) : base(context) { }
        #endregion

        #region Method
        public override async Task<Token> GetByIdAsync(int id)
        => await Context.Tokens.SingleOrDefaultAsync(x => x.Id.Equals(id));
        #endregion
    }
}
