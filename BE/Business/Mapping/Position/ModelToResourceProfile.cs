using AutoMapper;
using Business.Resources.Position;

namespace Business.Mapping.Office
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.Position, PositionResource>();
        }
    }
}
