using AutoMapper;
using HR_Management.Resources.Certificate;

namespace HR_Management.Mapping.Certificate
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateCertificateResource, Domain.Models.Certificate>();
            CreateMap<CertificateResource, Domain.Models.Certificate>();
            CreateMap<UpdateCertificateResource, Domain.Models.Certificate>();
        }
    }
}
