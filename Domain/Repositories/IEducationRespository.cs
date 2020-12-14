using HR_Management.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Domain.Repositories
{
    public interface IEducationRespository
    {
        Task<IEnumerable<Education>> ListAsync(int personId);
        Task AddAsync(Education education);
        void Update(Education education);
    }
}
