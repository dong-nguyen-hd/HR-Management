using AutoMapper;
using HR_Management.Resources.Location;

namespace HR_Management.Mapping.Location
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.Location, LocationResource>();
        }
    }
}
