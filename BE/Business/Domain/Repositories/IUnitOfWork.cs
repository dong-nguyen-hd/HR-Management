using System;
using System.Threading.Tasks;

namespace Business.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task CompleteAsync();
    }
}
