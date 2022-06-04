using AutoMapper;
using Business.Communication;
using Business.CustomException;
using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Position;
using Microsoft.Extensions.Options;

namespace Business.Services
{
    public class PositionService : BaseService<PositionResource, CreatePositionResource, UpdatePositionResource, Position>, IPositionService
    {
        #region Property
        private readonly IPositionRepository _positionRepository;
        #endregion

        #region Constructor
        public PositionService(IPositionRepository positionRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(positionRepository, mapper, unitOfWork, responseMessage)
        {
            this._positionRepository = positionRepository;
        }
        #endregion

        #region Method
        public override async Task<BaseResponse<PositionResource>> InsertAsync(CreatePositionResource createLocationResource)
        {
            try
            {
                // Validate position name is existent?
                var hasValue = await _positionRepository.FindByNameAsync(createLocationResource.Name, true);
                if (hasValue.Count > 0)
                    return new BaseResponse<PositionResource>(ResponseMessage.Values["Position_Existent"]);

                // Mapping Resource to Position
                var Position = Mapper.Map<CreatePositionResource, Position>(createLocationResource);

                await _positionRepository.InsertAsync(Position);
                await UnitOfWork.CompleteAsync();

                return new BaseResponse<PositionResource>(Mapper.Map<Position, PositionResource>(Position));
            }
            catch (Exception ex)
            {
                throw new MessageResultException(ResponseMessage.Values["Position_Saving_Error"], ex);
            }
        }
        #endregion
    }
}
