using AutoMapper;
using Business.Communication;
using Business.CustomException;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Resources;
using Microsoft.Extensions.Options;

namespace Business.Services
{
    public abstract class BaseService<Response, Insert, Update, Entity> : ResponseMessageService, IBaseService<Response, Insert, Update, Entity> where Entity : class
    {
        #region Property
        private readonly IBaseRepository<Entity> _baseRepository;
        protected readonly IMapper Mapper;
        protected readonly IUnitOfWork UnitOfWork;
        #endregion

        #region Constructor
        public BaseService(IBaseRepository<Entity> baseRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(responseMessage)
        {
            this._baseRepository = baseRepository;
            this.Mapper = mapper;
            this.UnitOfWork = unitOfWork;
        }
        #endregion

        #region Method
        public virtual async Task<BaseResponse<IEnumerable<Response>>> GetAllAsync()
        {
            // Get list record from DB
            var tempEntity = await _baseRepository.GetAllAsync();
            // Mapping Entity to Resource
            var result = Mapper.Map<IEnumerable<Entity>, IEnumerable<Response>>(tempEntity);

            return new BaseResponse<IEnumerable<Response>>(result);
        }

        public virtual async Task<BaseResponse<Response>> GetByIdAsync(int id)
        {
            var tempEntity = await _baseRepository.GetByIdAsync(id);
            // Mapping Entity to Resource
            var result = Mapper.Map<Entity, Response>(tempEntity);

            return new BaseResponse<Response>(result);
        }

        public virtual async Task<BaseResponse<Response>> InsertAsync(Insert insertResource)
        {
            try
            {
                // Mapping Resource to Entity
                var tempEntity = Mapper.Map<Insert, Entity>(insertResource);

                var personId = (int)tempEntity.GetType().GetProperty("PersonId").GetValue(tempEntity);
                int orderIndex = await _baseRepository.MaximumOrderIndexAsync(personId);
                tempEntity.GetType().GetProperty("OrderIndex").SetValue(tempEntity, orderIndex);

                await _baseRepository.InsertAsync(tempEntity);
                await UnitOfWork.CompleteAsync();

                return new BaseResponse<Response>(Mapper.Map<Entity, Response>(tempEntity));
            }
            catch (Exception ex)
            {
                throw new MessageResultException(ResponseMessage.Values["Saving_Error"], ex);
            }
        }

        public virtual async Task<BaseResponse<Response>> RemoveAsync(int id)
        {
            try
            {
                // Validate Id is existent?
                var tempEntity = await _baseRepository.GetByIdAsync(id);
                if (tempEntity is null)
                    return new BaseResponse<Response>(ResponseMessage.Values["Id_NoData"]);

                _baseRepository.Remove(tempEntity);
                await UnitOfWork.CompleteAsync();

                return new BaseResponse<Response>(Mapper.Map<Entity, Response>(tempEntity));
            }
            catch (Exception ex)
            {
                throw new MessageResultException(ResponseMessage.Values["Deleting_Error"], ex);
            }
        }

        public virtual async Task<DeleteResponse<IEnumerable<Response>>> RemoveRangeAsync(List<int> ids)
        {
            try
            {
                var tempEntity = await _baseRepository.GetWithPrimaryKeyAsync(ids);
                var totalDeleted = _baseRepository.RemoveRange(tempEntity);

                if (totalDeleted == 0 && ids.Count > 0)
                    return new DeleteResponse<IEnumerable<Response>>(ResponseMessage.Values["Deleting_Error"]);

                await UnitOfWork.CompleteAsync();

                // Mapping
                var resource = Mapper.Map<IEnumerable<Entity>, IEnumerable<Response>>(tempEntity);

                return new DeleteResponse<IEnumerable<Response>>(ids.Count, totalDeleted, resource);
            }
            catch (Exception ex)
            {
                throw new MessageResultException(ResponseMessage.Values["Deleting_Error"], ex);
            }
        }

        public virtual async Task<BaseResponse<Response>> UpdateAsync(int id, Update updateResource)
        {
            try
            {
                // Validate Id is existent?
                var tempEntity = await _baseRepository.GetByIdAsync(id);
                if (tempEntity is null)
                    return new BaseResponse<Response>(ResponseMessage.Values["NoData"]);
                // Update infomation
                Mapper.Map(updateResource, tempEntity);

                await UnitOfWork.CompleteAsync();
                // Mapping
                var resource = Mapper.Map<Entity, Response>(tempEntity);

                return new BaseResponse<Response>(resource);
            }
            catch (Exception ex)
            {
                throw new MessageResultException(ResponseMessage.Values["Updating_Error"], ex);
            }
        }

        public virtual async Task<BaseResponse<Response>> SwapAsync(SwapResource swapResource)
        {
            try
            {
                // Validate Id duplicate
                if (swapResource.CurrentId == swapResource.TurnedId)
                    return new BaseResponse<Response>(ResponseMessage.Values["Swap_Id_Invalid"]);
                // Validate Id is existent?
                var currentEntity = await _baseRepository.GetByIdAsync(swapResource.CurrentId);
                var turnedEntity = await _baseRepository.GetByIdAsync(swapResource.TurnedId);
                if (currentEntity is null || turnedEntity is null)
                    return new BaseResponse<Response>(ResponseMessage.Values["NoData"]);

                var currentEntityValue = (int)currentEntity.GetType().GetProperty("PersonId").GetValue(currentEntity);
                var turnedEntityValue = (int)turnedEntity.GetType().GetProperty("PersonId").GetValue(turnedEntity);

                if (currentEntityValue != turnedEntityValue)
                    return new BaseResponse<Response>(ResponseMessage.Values["Swap_Id_Invalid"]);

                // Swap property OrderIndex
                var currentOrderIndex = (int)currentEntity.GetType().GetProperty("OrderIndex").GetValue(currentEntity);
                var turnedOrderIndex = (int)turnedEntity.GetType().GetProperty("OrderIndex").GetValue(turnedEntity);

                currentEntity.GetType().GetProperty("OrderIndex").SetValue(currentEntity, turnedOrderIndex);
                turnedEntity.GetType().GetProperty("OrderIndex").SetValue(turnedEntity, currentOrderIndex);

                await UnitOfWork.CompleteAsync();

                return new BaseResponse<Response>(true);
            }
            catch (Exception ex)
            {
                throw new MessageResultException(ResponseMessage.Values["Swapping_Error"], ex);
            }
        }
        #endregion
    }
}
