using HR_Management.Domain.Models;
using HR_Management.Domain.Services.Communication;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface IEducationService
    {
        Task<EducationResponse> ListAsync(int personId);
        Task<EducationResponse> CreateAsync(Education education);
        Task<EducationResponse> UpdateAsync(int id, Education education);
        Task<EducationResponse> DeleteAsync(int id);
    }
}
