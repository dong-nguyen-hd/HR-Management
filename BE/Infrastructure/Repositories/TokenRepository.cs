using Business.Domain.Models;
using Business.Domain.Repositories;
using Infrastructure.Contexts;

namespace Infrastructure.Repositories
{
    public class TokenRepository : BaseRepository<Token>, ITokenRepository
    {
        #region Constructor
        public TokenRepository(AppDbContext context) : base(context) { }
        #endregion

        #region Method
        
        #endregion
    }
}
