using AutoMapper;
using Business.Communication;
using Business.CustomException;
using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Location;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Business.Services
{
    public class LocationService : BaseService<LocationResource, CreateLocationResource, UpdateLocationResource, Location>, ILocationService
    {
        #region Property
        private readonly ILocationRepository _locationRepository;
        #endregion

        #region Constructor
        public LocationService(ILocationRepository locationRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(locationRepository, mapper, unitOfWork, responseMessage)
        {
            this._locationRepository = locationRepository;
        }
        #endregion

        #region Method
        public override async Task<BaseResponse<LocationResource>> InsertAsync(CreateLocationResource createLocationResource)
        {
            try
            {
                // Mapping Resource to Location
                var location = Mapper.Map<CreateLocationResource, Location>(createLocationResource);

                await _locationRepository.InsertAsync(location);
                await UnitOfWork.CompleteAsync();

                return new BaseResponse<LocationResource>(Mapper.Map<Location, LocationResource>(location));
            }
            catch (Exception ex)
            {
                throw new MessageResultException(ResponseMessage.Values["Location_Saving_Error"], ex);
            }
        }
        #endregion
    }
}
