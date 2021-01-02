using HR_Management.Domain.Models;

namespace HR_Management.Domain.Services.Communication
{
    public class CertificateResponse : BaseResponse<Certificate>
    {
        public CertificateResponse(bool isSuccess) : base(isSuccess) { }

        public CertificateResponse(Certificate certificate) : base(certificate) { }

        public CertificateResponse(object objCertificate) : base(objCertificate) { }

        public CertificateResponse(string message) : base(message) { }
    }
}
