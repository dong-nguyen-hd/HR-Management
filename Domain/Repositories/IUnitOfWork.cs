using System.Threading.Tasks;

namespace HR_Management.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}
