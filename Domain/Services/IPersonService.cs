using HR_Management.Domain.Services.Communication;
using HR_Management.Resources;
using HR_Management.Resources.Person;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface IPersonService
    {
        Task<PersonResponse<PersonResource>> FindByIdAsync(int id);
        Task<PersonResponse<PersonResource>> CreateAsync(CreatePersonResource createPersonResource);
        Task<PersonResponse<PersonResource>> UpdateAsync(int id, UpdatePersonResource updatePersonResource);
        Task<PersonResponse<PersonResource>> DeleteAsync(int id);
        Task<PersonResponse<PersonResource>> AssignComponentAsync(int id, ComponentResource component);
    }
}
