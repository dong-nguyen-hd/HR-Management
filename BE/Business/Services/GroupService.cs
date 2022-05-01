using AutoMapper;
using Business.Communication;
using Business.CustomException;
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
            this._groupRepository = groupRepository;
        }
        #endregion

        #region Property
        private readonly IGroupRepository _groupRepository;
        #endregion

        #region Method
        public override async Task<BaseResponse<GroupResource>> InsertAsync(CreateGroupResource createGroupResource)
        {
            try
            {
                var tempGroup = Mapper.Map<CreateGroupResource, Group>(createGroupResource);

                await _groupRepository.InsertAsync(tempGroup);
                await UnitOfWork.CompleteAsync();

                return new BaseResponse<GroupResource>(Mapper.Map<Group, GroupResource>(tempGroup));
            }
            catch (Exception ex)
            {
                throw new MessageResultException(ResponseMessage.Values["Group_Saving_Error"], ex);
            }
        }
        #endregion
    }
}
