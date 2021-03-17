using AutoMapper;
using HR_Management.Extensions;
using HR_Management.Infrastructure;
using HR_Management.Resources.Person;

namespace HR_Management.Mapping.Person
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreatePersonResource, Domain.Models.Person>()
                .ForMember(x => x.FirstName, opt => opt.MapFrom(src => src.FirstName.RemoveSpaceCharacter()))
                .ForMember(x => x.LastName, opt => opt.MapFrom(src => src.LastName.RemoveSpaceCharacter()))
                .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Email.RemoveSpaceCharacter()))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description.RemoveSpaceCharacter()))
                .ForMember(x => x.Phone, opt => opt.MapFrom(src => src.Phone.RemoveSpaceCharacter()))
                .ForMember(x => x.OrderIndex, opt => opt.MapFrom(src => DefaultOrderIndexPerson())); // Use Default value while create a person record

            CreateMap<PersonResource, Domain.Models.Person>()
                .ForMember(x => x.FirstName, opt => opt.MapFrom(src => src.FirstName.RemoveSpaceCharacter()))
                .ForMember(x => x.LastName, opt => opt.MapFrom(src => src.LastName.RemoveSpaceCharacter()))
                .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Email.RemoveSpaceCharacter()))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description.RemoveSpaceCharacter()))
                .ForMember(x => x.Phone, opt => opt.MapFrom(src => src.Phone.RemoveSpaceCharacter()));

            CreateMap<UpdatePersonResource, Domain.Models.Person>()
                .ForMember(x => x.FirstName, opt => opt.MapFrom(src => src.FirstName.RemoveSpaceCharacter()))
                .ForMember(x => x.LastName, opt => opt.MapFrom(src => src.LastName.RemoveSpaceCharacter()))
                .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Email.RemoveSpaceCharacter()))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description.RemoveSpaceCharacter()))
                .ForMember(x => x.Phone, opt => opt.MapFrom(src => src.Phone.RemoveSpaceCharacter()));
        }

        string DefaultOrderIndexPerson()
            => string.Format($"{(int)eOrder.WorkHistory},{(int)eOrder.Skill},{(int)eOrder.Education},{(int)eOrder.Certificate},{(int)eOrder.Project}");
    }
}
