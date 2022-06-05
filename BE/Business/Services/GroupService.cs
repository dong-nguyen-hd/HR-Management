using AutoMapper;
using Business.Communication;
using Business.CustomException;
using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Extensions;
using Business.Resources;
using Business.Resources.Group;
using Business.Resources.Technology;
using Microsoft.Extensions.Options;

namespace Business.Services
{
    public class GroupService : BaseService<GroupResource, CreateGroupResource, UpdateGroupResource, Group>, IGroupService
    {
        #region Constructor
        public GroupService(IGroupRepository groupRepository,
            ITechnologyService technologyService,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(groupRepository, mapper, unitOfWork, responseMessage)
        {
            this._groupRepository = groupRepository;
            this._technologyService = technologyService;
        }
        #endregion

        #region Property
        private readonly IGroupRepository _groupRepository;
        private readonly ITechnologyService _technologyService;
        #endregion

        #region Method
        public override async Task<BaseResponse<GroupResource>> InsertAsync(CreateGroupResource createGroupResource)
        {
            try
            {
                var tempGroup = Mapper.Map<CreateGroupResource, Group>(createGroupResource);

                await _groupRepository.InsertAsync(tempGroup);
                await UnitOfWork.CompleteAsync();

                // Mapping
                var totalTechnology = await _technologyService.GetAllAsync();
                var tempResource = ConvertGroupResource(totalTechnology.Resource, tempGroup);

                return new BaseResponse<GroupResource>(tempResource);
            }
            catch (Exception ex)
            {
                throw new MessageResultException(ResponseMessage.Values["Group_Saving_Error"], ex);
            }
        }

        public override async Task<BaseResponse<GroupResource>> UpdateAsync(int id, UpdateGroupResource updateGroupResource)
        {
            try
            {
                // Validate Id is existent?
                var tempGroup = await _groupRepository.GetByIdAsync(id);
                if (tempGroup is null)
                    return new BaseResponse<GroupResource>(ResponseMessage.Values["Group_NoData"]);

                // Update infomation
                Mapper.Map(updateGroupResource, tempGroup);
                await UnitOfWork.CompleteAsync();

                // Mapping
                var totalTechnology = await _technologyService.GetAllAsync();
                var tempResource = ConvertGroupResource(totalTechnology.Resource, tempGroup);

                return new BaseResponse<GroupResource>(tempResource);
            }
            catch (Exception ex)
            {
                throw new MessageResultException(ResponseMessage.Values["Group_Updating_Error"], ex);
            }
        }

        public async Task<PaginationResponse<IEnumerable<GroupResource>>> GetPaginationAsync(QueryResource pagination, FilterGroupResource filterResource)
        {
            var totalTechnology = await _technologyService.GetAllAsync();
            var paginationPerson = await _groupRepository.GetPaginationAsync(pagination, filterResource);

            // Mapping
            var tempResource = ConvertGroupResource(totalTechnology.Resource, paginationPerson.records);

            var resource = new PaginationResponse<IEnumerable<GroupResource>>(tempResource);

            // Using extension-method for pagination
            resource.CreatePaginationResponse(pagination, paginationPerson.total);

            return resource;
        }

        #region Private work
        private IEnumerable<GroupResource> ConvertGroupResource(IEnumerable<TechnologyResource> totalTechnology, IEnumerable<Group> totalGroup)
        {
            List<GroupResource> listGroupResource = new List<GroupResource>(totalGroup.Count());

            foreach (var group in totalGroup)
            {
                var tempGroupResource = ConvertGroupResource(totalTechnology, group);

                listGroupResource.Add(tempGroupResource);
            }

            return listGroupResource;
        }

        private GroupResource ConvertGroupResource(IEnumerable<TechnologyResource> totalTechnology, Group group)
        {
            var tempGroupResource = Mapper.Map<Group, GroupResource>(group);

            if (!string.IsNullOrEmpty(group.Technologies))
                tempGroupResource.Technologies = totalTechnology.IntersectTechnology(group.Technologies);

            return tempGroupResource;
        }
        #endregion

        #endregion
    }
}
