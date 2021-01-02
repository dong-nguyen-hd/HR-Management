#nullable enable
using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using HR_Management.Domain.Services;
using HR_Management.Domain.Services.Communication;
using HR_Management.Extensions;
using System;
using System.Threading.Tasks;

namespace HR_Management.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LocationService(ILocationRepository locationRepository, IUnitOfWork unitOfWork)
        {
            this._locationRepository = locationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<LocationResponse> CreateAsync(Location location)
        {
            // Assign value
            location.Address = location.Address.RemoveSpaceCharacter();
            location.City = location.City.RemoveSpaceCharacter();
            location.Country = location.Country.RemoveSpaceCharacter();

            try
            {
                await _locationRepository.AddAsync(location);
                await _unitOfWork.CompleteAsync();

                return new LocationResponse(location);
            }
            catch (Exception ex)
            {
                return new LocationResponse($"An error occurred when saving the location: {ex.Message}");
            }
        }

        public async Task<LocationResponse> DeleteAsync(int id)
        {
            // Validate Id is existent?
            var tempLocation = await _locationRepository.FindByIdAsync(id);
            if (tempLocation is null)
                return new LocationResponse("Location not exist.");
            // Change property Status: true -> false
            tempLocation.Status = false;

            try
            {
                await _unitOfWork.CompleteAsync();

                return new LocationResponse(tempLocation);
            }
            catch (Exception ex)
            {
                return new LocationResponse($"An error occurred when deleting the location: {ex.Message}");
            }
        }

        public async Task<LocationResponse> ListAsync()
        {
            // Get list record from DB
            var temp = await _locationRepository.ListAsync();

            return new LocationResponse(temp);
        }

        public async Task<LocationResponse> UpdateAsync(int id, Location location)
        {
            // Validate Id is existent?
            var tempLocation = await _locationRepository.FindByIdAsync(id);
            if (tempLocation is null)
                return new LocationResponse("Location not exist.");
            // Update infomation
            tempLocation.Address = location.Address.RemoveSpaceCharacter();
            tempLocation.City = location.City.RemoveSpaceCharacter();
            tempLocation.Country = location.Country;

            try
            {
                await _unitOfWork.CompleteAsync();

                return new LocationResponse(tempLocation);
            }
            catch (Exception ex)
            {
                return new LocationResponse($"An error occurred when updating the location: {ex.Message}");
            }
        }
    }
}
