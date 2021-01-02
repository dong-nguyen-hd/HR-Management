using AutoMapper;
using HR_Management.Resources.WorkHistory;

namespace HR_Management.Mapping.WorkHistory
{
    public class ModelToResourceProfile : Profile
    {
        public ModelToResourceProfile()
        {
            CreateMap<Domain.Models.WorkHistory, WorkHistoryResource>();
        }
    }
}
