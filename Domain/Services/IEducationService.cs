using HR_Management.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface IEducationService
    {
        Task<IEnumerable<Education>> ListAsync(int personId);
    }
}
