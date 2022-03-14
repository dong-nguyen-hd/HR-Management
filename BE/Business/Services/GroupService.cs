using AutoMapper;
using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Group;
using Microsoft.Extensions.Options;

namespace Business.Services
{
    public class GroupService : BaseService<GroupResource, CreateGroupResource, UpdateGroupResource, Group>, IGroupService
    {
        #region Constructor
        public GroupService(IGroupRepository groupRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(groupRepository, mapper, unitOfWork, responseMessage)
        {
        }
        #endregion
    }
}
