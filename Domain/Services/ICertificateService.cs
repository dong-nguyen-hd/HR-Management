using HR_Management.Domain.Models;
using HR_Management.Domain.Services.Communication;
using HR_Management.Resources;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface ICertificateService
    {
        Task<CertificateResponse> ListAsync(int personId);
        Task<CertificateResponse> CreateAsync(Certificate certificate);
        Task<CertificateResponse> UpdateAsync(int id, Certificate certificate);
        Task<CertificateResponse> DeleteAsync(int id);
        Task<CertificateResponse> SwapAsync(SwapResource obj);
    }
}
