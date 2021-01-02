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
    public class TechnologyService : ITechnologyService
    {
        private readonly ITechnologyRepository _technologyRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TechnologyService(ITechnologyRepository technologyRepository,
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            this._technologyRepository = technologyRepository;
            this._categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TechnologyResponse> ListAsync()
        {
            // Get list record from DB
            var temp = await _technologyRepository.ListAsync();

            return new TechnologyResponse(temp);
        }

        public async Task<TechnologyResponse> ListAsync(int categoryId)
        {
            // Validate category is existent?
            var tempPerson = await _categoryRepository.FindByIdAsync(categoryId);
            if (tempPerson is null)
                return new TechnologyResponse($"CategoryId '{categoryId}' not exist.");
            // Get list record from DB
            var temp = await _technologyRepository.ListAsync(categoryId);

            return new TechnologyResponse(temp);
        }

        public async Task<TechnologyResponse> CreateAsync(Technology technology)
        {
            // Validate category is existent?
            var tempPerson = await _categoryRepository.FindByIdAsync(technology.CategoryId);
            if (tempPerson is null)
                return new TechnologyResponse($"CategoryId '{technology.CategoryId}' not exist.");
            // Assign value
            technology.Name = technology.Name.RemoveSpaceCharacter();
            
            try
            {
                await _technologyRepository.AddAsync(technology);
                await _unitOfWork.CompleteAsync();

                return new TechnologyResponse(technology);
            }
            catch (Exception ex)
            {
                return new TechnologyResponse($"An error occurred when saving the technology: {ex.Message}");
            }
        }

        public async Task<TechnologyResponse> UpdateAsync(int id, Technology technology)
        {
            // Validate Id is existent?
            var tempTechnology = await _technologyRepository.FindByIdAsync(id);
            if (tempTechnology is null)
                return new TechnologyResponse("Technology not exist.");
            // Update infomation
            tempTechnology.Name = technology.Name.RemoveSpaceCharacter();
            
            try
            {
                await _unitOfWork.CompleteAsync();

                return new TechnologyResponse(tempTechnology);
            }
            catch (Exception ex)
            {
                return new TechnologyResponse($"An error occurred when updating the technology: {ex.Message}");
            }
        }

        public async Task<TechnologyResponse> DeleteAsync(int id)
        {
            // Validate Id is existent?
            var tempTechnology = await _technologyRepository.FindByIdAsync(id);
            if (tempTechnology is null)
                return new TechnologyResponse("Technology not exist.");
            // Change property Status: true -> false
            tempTechnology.Status = false;

            try
            {
                await _unitOfWork.CompleteAsync();

                return new TechnologyResponse(tempTechnology);
            }
            catch (Exception ex)
            {
                return new TechnologyResponse($"An error occurred when deleting the technology: {ex.Message}");
            }
        }
    }
}
