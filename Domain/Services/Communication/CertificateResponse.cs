namespace HR_Management.Domain.Services.Communication
{
    public class CertificateResponse<T> : BaseResponse<T>
    {
        public CertificateResponse(T resource) : base(resource) { }

        public CertificateResponse(string message) : base(message) { }
    }
}
