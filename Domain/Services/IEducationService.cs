using HR_Management.Domain.Services.Communication;
using HR_Management.Resources;
using HR_Management.Resources.Education;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface IEducationService
    {
        Task<EducationResponse<IEnumerable<EducationResource>>> ListAsync(int personId);
        Task<EducationResponse<EducationResource>> CreateAsync(CreateEducationResource resource);
        Task<EducationResponse<EducationResource>> UpdateAsync(int id, UpdateEducationResource resource);
        Task<EducationResponse<EducationResource>> DeleteAsync(int id);
        Task<EducationResponse<EducationResource>> SwapAsync(SwapResource obj);
    }
}
