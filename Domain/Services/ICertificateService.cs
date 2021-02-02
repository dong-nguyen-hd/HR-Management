using HR_Management.Domain.Services.Communication;
using HR_Management.Resources;
using HR_Management.Resources.Certificate;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface ICertificateService
    {
        Task<CertificateResponse<IEnumerable<CertificateResource>>> ListAsync(int personId);
        Task<CertificateResponse<CertificateResource>> CreateAsync(CreateCertificateResource resource);
        Task<CertificateResponse<CertificateResource>> UpdateAsync(int id, UpdateCertificateResource resource);
        Task<CertificateResponse<CertificateResource>> DeleteAsync(int id);
        Task<CertificateResponse<CertificateResource>> SwapAsync(SwapResource obj);
    }
}
