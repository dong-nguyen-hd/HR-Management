using HR_Management.Domain.Services.Communication;
using HR_Management.Resources;
using HR_Management.Resources.WorkHistory;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface IWorkHistoryService
    {
        Task<WorkHistoryResponse<IEnumerable<WorkHistoryResource>>> ListAsync(int personId);
        Task<WorkHistoryResponse<WorkHistoryResource>> CreateAsync(CreateWorkHistoryResource resource);
        Task<WorkHistoryResponse<WorkHistoryResource>> UpdateAsync(int id, UpdateWorkHistoryResource resource);
        Task<WorkHistoryResponse<WorkHistoryResource>> DeleteAsync(int id);
        Task<WorkHistoryResponse<WorkHistoryResource>> SwapAsync(SwapResource obj);
    }
}
