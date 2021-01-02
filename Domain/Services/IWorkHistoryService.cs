using HR_Management.Domain.Models;
using HR_Management.Domain.Services.Communication;
using HR_Management.Resources;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface IWorkHistoryService
    {
        Task<WorkHistoryResponse> ListAsync(int personId);
        Task<WorkHistoryResponse> CreateAsync(WorkHistory workHistory);
        Task<WorkHistoryResponse> UpdateAsync(int id, WorkHistory workHistory);
        Task<WorkHistoryResponse> DeleteAsync(int id);
        Task<WorkHistoryResponse> SwapAsync(SwapResource obj);
    }
}
