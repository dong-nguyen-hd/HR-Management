using AutoMapper;
using HR_Management.Resources.WorkHistory;

namespace HR_Management.Mapping.WorkHistory
{
    public class ResourceToModelProfile : Profile
    {
        public ResourceToModelProfile()
        {
            CreateMap<CreateWorkHistoryResource, Domain.Models.WorkHistory>();
            CreateMap<WorkHistoryResource, Domain.Models.WorkHistory>();
            CreateMap<UpdateWorkHistoryResource, Domain.Models.WorkHistory>();
        }
    }
}
