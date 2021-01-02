using HR_Management.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Domain.Repositories
{
    public interface ICertificateRepository
    {
        Task<IEnumerable<Certificate>> ListAsync(int personId);
        Task AddAsync(Certificate certificate);
        void Update(Certificate certificate);
        Task<Certificate> FindByIdAsync(int id);
        void Remove(Certificate certificate);
        Task<int> MaximumOrderIndexAsync(int personId);
    }
}
