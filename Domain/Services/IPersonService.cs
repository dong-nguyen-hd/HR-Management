using HR_Management.Domain.Services.Communication;
using HR_Management.Resources;
using HR_Management.Resources.Person;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface IPersonService
    {
        Task<PersonResponse<PersonResource>> FindByIdAsync(int id, bool isMobile = false);
        Task<PersonResponse<PersonResource>> CreateAsync(CreatePersonResource createPersonResource, bool isMobile = false);
        Task<PersonResponse<PersonResource>> UpdateAsync(int id, UpdatePersonResource updatePersonResource, bool isMobile = false);
        Task<PersonResponse<PersonResource>> DeleteAsync(int id, bool isMobile = false);
        Task<PersonResponse<PersonResource>> AssignComponentAsync(int id, ComponentResource component);
    }
}
