#nullable enable
using AutoMapper;
using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using HR_Management.Domain.Services;
using HR_Management.Domain.Services.Communication;
using HR_Management.Resources.Location;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LocationService(ILocationRepository locationRepository, 
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this._locationRepository = locationRepository;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<LocationResponse<LocationResource>> CreateAsync(CreateLocationResource createLocationResource)
        {
            // Mapping Resource to Location
            var location = _mapper.Map<CreateLocationResource, Location>(createLocationResource);

            try
            {
                await _locationRepository.AddAsync(location);
                await _unitOfWork.CompleteAsync();

                // Mapping
                var resource = _mapper.Map<Location, LocationResource>(location);

                return new LocationResponse<LocationResource>(resource);
            }
            catch (Exception ex)
            {
                return new LocationResponse<LocationResource>($"An error occurred when saving the Location: {ex.Message}");
            }
        }

        public async Task<LocationResponse<LocationResource>> DeleteAsync(int id)
        {
            // Validate Id is existent?
            var tempLocation = await _locationRepository.FindByIdAsync(id);
            if (tempLocation is null)
                return new LocationResponse<LocationResource>("Location is not existent.");
            // Change property Status: true -> false
            tempLocation.Status = false;

            try
            {
                await _unitOfWork.CompleteAsync();
                // Mapping
                var resource = _mapper.Map<Location, LocationResource>(tempLocation);

                return new LocationResponse<LocationResource>(resource);
            }
            catch (Exception ex)
            {
                return new LocationResponse<LocationResource>($"An error occurred when deleting the Location: {ex.Message}");
            }
        }

        public async Task<LocationResponse<LocationResource>> FindAsync(int id)
        {
            var tempLocation = await _locationRepository.FindByIdAsync(id);
            // Mapping Project to Resource
            var resource = _mapper.Map<Location, LocationResource>(tempLocation);

            return new LocationResponse<LocationResource>(resource);
        }

        public async Task<LocationResponse<IEnumerable<LocationResource>>> ListAsync()
        {
            // Get list record from DB
            var tempLocation = await _locationRepository.ListAsync();
            // Mapping Project to Resource
            var resource = _mapper.Map<IEnumerable<Location>, IEnumerable<LocationResource>>(tempLocation);

            return new LocationResponse<IEnumerable<LocationResource>>(resource);
        }

        public async Task<LocationResponse<LocationResource>> UpdateAsync(int id, UpdateLocationResource updateLocationResource)
        {
            // Validate Id is existent?
            var tempLocation = await _locationRepository.FindByIdAsync(id);
            if (tempLocation is null)
                return new LocationResponse<LocationResource>("Location is not existent.");
            // Update infomation
            _mapper.Map(updateLocationResource, tempLocation);

            try
            {
                await _unitOfWork.CompleteAsync();
                // Mapping
                var resource = _mapper.Map<Location, LocationResource>(tempLocation);

                return new LocationResponse<LocationResource>(resource);
            }
            catch (Exception ex)
            {
                return new LocationResponse<LocationResource>($"An error occurred when updating the Location: {ex.Message}");
            }
        }
    }
}
