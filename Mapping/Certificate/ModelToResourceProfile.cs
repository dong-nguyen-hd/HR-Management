using AutoMapper;
using HR_Management.Resources.Certificate;

namespace HR_Management.Mapping.Certificate
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.Certificate, CertificateResource>();
        }
    }
}
