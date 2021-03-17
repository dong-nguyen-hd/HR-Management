using HR_Management.Domain.Services.Communication;
using HR_Management.Resources;
using HR_Management.Resources.Person;
using HR_Management.Resources.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface IPersonService
    {
        Task<PersonResponse<IEnumerable<PersonResource>>> ListAsync(QueryResource pagintation);
        Task<PersonResponse<IEnumerable<PersonResource>>> ListWithLocationAsync(QueryResource pagintation, int locationId);
        Task<PersonResponse<PersonResource>> FindByIdAsync(int id);
        Task<int> TotalRecordAsync();
        Task<PersonResponse<PersonResource>> CreateAsync(CreatePersonResource createPersonResource);
        Task<PersonResponse<PersonResource>> UpdateAsync(int id, UpdatePersonResource updatePersonResource);
        Task<PersonResponse<PersonResource>> DeleteAsync(int id);
        Task<PersonResponse<PersonResource>> AssignComponentAsync(int id, ComponentResource component);
    }
}
