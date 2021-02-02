using AutoMapper;
using HR_Management.Extensions;
using HR_Management.Resources.Certificate;

namespace HR_Management.Mapping.Certificate
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateCertificateResource, Domain.Models.Certificate>()
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name.RemoveSpaceCharacter()))
                .ForMember(x => x.Provider, opt => opt.MapFrom(src => src.Provider.RemoveSpaceCharacter()));

            CreateMap<CertificateResource, Domain.Models.Certificate>()
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name.RemoveSpaceCharacter()))
                .ForMember(x => x.Provider, opt => opt.MapFrom(src => src.Provider.RemoveSpaceCharacter()));

            CreateMap<UpdateCertificateResource, Domain.Models.Certificate>()
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name.RemoveSpaceCharacter()))
                .ForMember(x => x.Provider, opt => opt.MapFrom(src => src.Provider.RemoveSpaceCharacter()));
        }
    }
}
