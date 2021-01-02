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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            this._categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryResponse> ListAsync()
        {
            // Get list record from DB
            var temp = await _categoryRepository.ListAsync();

            return new CategoryResponse(temp);
        }

        public async Task<CategoryResponse> CreateAsync(Category category)
        {
            // Assign value
            category.Name = category.Name.RemoveSpaceCharacter();
            
            try
            {
                await _categoryRepository.AddAsync(category);
                await _unitOfWork.CompleteAsync();

                return new CategoryResponse(category);
            }
            catch (Exception ex)
            {
                return new CategoryResponse($"An error occurred when saving the category: {ex.Message}");
            }
        }

        public async Task<CategoryResponse> UpdateAsync(int id, Category category)
        {
            // Validate Id is existent?
            var tempCategory = await _categoryRepository.FindByIdAsync(id);
            if (tempCategory is null)
                return new CategoryResponse("Category not exist.");
            // Update infomation
            tempCategory.Name = category.Name.RemoveSpaceCharacter();
            
            try
            {
                await _unitOfWork.CompleteAsync();

                return new CategoryResponse(tempCategory);
            }
            catch (Exception ex)
            {
                return new CategoryResponse($"An error occurred when updating the category: {ex.Message}");
            }
        }

        public async Task<CategoryResponse> DeleteAsync(int id)
        {
            // Validate Id is existent?
            var tempCategory = await _categoryRepository.FindByIdAsync(id);
            if (tempCategory is null)
                return new CategoryResponse("Category not exist.");
            // Change property Status: true -> false
            tempCategory.Status = false;

            try
            {
                await _unitOfWork.CompleteAsync();

                return new CategoryResponse(tempCategory);
            }
            catch (Exception ex)
            {
                return new CategoryResponse($"An error occurred when deleting the category: {ex.Message}");
            }
        }
    }
}
