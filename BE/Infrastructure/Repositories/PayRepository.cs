using Business.Domain.Models;
using Business.Domain.Repositories;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PayRepository : BaseRepository<Pay>, IPayRepository
    {
        #region Constructor
        public PayRepository(AppDbContext context) : base(context) { }
        #endregion

        #region Method
        
        #endregion
    }
}
