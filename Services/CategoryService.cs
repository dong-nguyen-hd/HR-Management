#nullable enable
using AutoMapper;
using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using HR_Management.Domain.Services;
using HR_Management.Domain.Services.Communication;
using HR_Management.Resources.Category;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(ICategoryRepository categoryRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            this._categoryRepository = categoryRepository;
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }

        public async Task<CategoryResponse<IEnumerable<CategoryResource>>> ListAsync()
        {
            // Get list record from DB
            var tempCategory = await _categoryRepository.ListAsync();
            // Mapping
            var tempResource = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryResource>>(tempCategory);
            
            return new CategoryResponse<IEnumerable<CategoryResource>>(tempResource);
        }

        public async Task<CategoryResponse<CategoryResource>> CreateAsync(CreateCategoryResource createResource)
        {
            // Mapping
            var category = _mapper.Map<CreateCategoryResource, Category>(createResource);

            try
            {
                await _categoryRepository.AddAsync(category);
                await _unitOfWork.CompleteAsync();
                // Mapping
                var resource = _mapper.Map<Category, CategoryResource>(category);

                return new CategoryResponse<CategoryResource>(resource);
            }
            catch (Exception ex)
            {
                return new CategoryResponse<CategoryResource>($"An error occurred when saving the Category: {ex.Message}");
            }
        }

        public async Task<CategoryResponse<CategoryResource>> UpdateAsync(int id, UpdateCategoryResource updateResource)
        {
            // Validate Id is existent?
            var tempCategory = await _categoryRepository.FindByIdAsync(id);
            if (tempCategory is null)
                return new CategoryResponse<CategoryResource>("Category is not existent.");
            // Update infomation
            _mapper.Map(updateResource, tempCategory);

            try
            {
                await _unitOfWork.CompleteAsync();
                // Mapping
                var resource = _mapper.Map<Category, CategoryResource>(tempCategory);

                return new CategoryResponse<CategoryResource>(resource);
            }
            catch (Exception ex)
            {
                return new CategoryResponse<CategoryResource>($"An error occurred when updating the Category: {ex.Message}");
            }
        }

        public async Task<CategoryResponse<CategoryResource>> DeleteAsync(int id)
        {
            // Validate Id is existent?
            var tempCategory = await _categoryRepository.FindByIdAsync(id);
            if (tempCategory is null)
                return new CategoryResponse<CategoryResource>("Category is not existent.");
            // Change property Status: true -> false
            tempCategory.Status = false;

            try
            {
                await _unitOfWork.CompleteAsync();
                // Mapping
                var resource = _mapper.Map<Category, CategoryResource>(tempCategory);

                return new CategoryResponse<CategoryResource>(resource);
            }
            catch (Exception ex)
            {
                return new CategoryResponse<CategoryResource>($"An error occurred when deleting the Category: {ex.Message}");
            }
        }
    }
}
