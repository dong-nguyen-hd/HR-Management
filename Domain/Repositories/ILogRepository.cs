using HR_Management.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Domain.Repositories
{
    public interface ILogRepository
    {
        Task<IEnumerable<Log>> ListAsync(int personId);
        Task AddAsync(Log log);
    }
}
