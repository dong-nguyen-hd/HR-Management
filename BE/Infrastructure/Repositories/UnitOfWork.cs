using Business.Domain.Repositories;
using Infrastructure.Contexts;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Property
        private readonly AppDbContext _context;
        #endregion

        #region Constructor
        public UnitOfWork(AppDbContext context)
        {
            this._context = context;
        }
        #endregion

        #region Method
        public async Task CompleteAsync()
        => await _context.SaveChangesAsync();

        public void Dispose()
        => _context.Dispose();
        #endregion
    }
}
