﻿using AutoMapper;
using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Location;
using Microsoft.Extensions.Options;

namespace Business.Services
{
    public class LocationService : BaseService<LocationResource, CreateLocationResource, UpdateLocationResource, Location>, ILocationService
    {
        #region Constructor
        public LocationService(ILocationRepository locationRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(locationRepository, mapper, unitOfWork, responseMessage)
        {
        }
        #endregion
    }
}
