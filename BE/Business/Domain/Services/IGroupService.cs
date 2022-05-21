using Business.Communication;
using Business.Domain.Models;
using Business.Resources;
using Business.Resources.Group;

namespace Business.Domain.Services
{
    public interface IGroupService : IBaseService<GroupResource, CreateGroupResource, UpdateGroupResource, Group>
    {
        Task<PaginationResponse<IEnumerable<GroupResource>>> GetPaginationAsync(QueryResource pagination, FilterGroupResource filterResource);
    }
}
