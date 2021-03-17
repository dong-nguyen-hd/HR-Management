using HR_Management.Domain.Services.Communication;
using HR_Management.Resources.Information;
using HR_Management.Resources.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface IInformationService
    {
        Task<InformationResponse<IEnumerable<InformationResource>>> ListWithPaginationAsync(QueryResource pagintation, string route);
    }
}
