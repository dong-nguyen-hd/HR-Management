using Business.Domain.Repositories;
using Infrastructure.Contexts;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Property
        private readonly AppDbContext _context;
        private bool _disposed;
        #endregion

        #region Constructor
        public UnitOfWork(AppDbContext context) =>
            this._context = context;
        #endregion

        #region Method
        public async Task CompleteAsync() =>
            await _context.SaveChangesAsync();

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
