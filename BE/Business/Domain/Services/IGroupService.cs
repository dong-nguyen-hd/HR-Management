using Business.Domain.Models;
using Business.Resources.Group;

namespace Business.Domain.Services
{
    public interface IGroupService : IBaseService<GroupResource, CreateGroupResource, UpdateGroupResource, Group>
    {
    }
}
