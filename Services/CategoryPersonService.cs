#nullable enable
using AutoMapper;
using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using HR_Management.Domain.Services;
using HR_Management.Domain.Services.Communication;
using HR_Management.Extensions;
using HR_Management.Resources;
using HR_Management.Resources.CategoryPerson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Services
{
    public class CategoryPersonService : ICategoryPersonService
    {
        private readonly ICategoryPersonRepository _categoryPersonRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITechnologyRepository _technologyRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryPersonService(ICategoryPersonRepository categoryPersonRepository,
            ICategoryRepository categoryRepository,
            ITechnologyRepository technologyRepository,
            IPersonRepository personRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            this._categoryPersonRepository = categoryPersonRepository;
            this._categoryRepository = categoryRepository;
            this._technologyRepository = technologyRepository;
            this._personRepository = personRepository;
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }

        public async Task<CategoryPersonResponse<IEnumerable<CategoryPersonResource>>> ListAsync(int personId)
        {
            // Validate person is existent?
            var tempPerson = await _personRepository.FindByIdAsync(personId);
            if (tempPerson is null)
                return new CategoryPersonResponse<IEnumerable<CategoryPersonResource>>($"Person Id '{personId}' is not existent.");
            // Get list record from DB
            var tempCategoryPerson = await _categoryPersonRepository.ListAsync(personId);

            // Mapping Project to Resource
            var resource = _mapper.Map<IEnumerable<CategoryPerson>, IEnumerable<CategoryPersonResource>>(tempCategoryPerson);

            return new CategoryPersonResponse<IEnumerable<CategoryPersonResource>>(resource);
        }

        public async Task<CategoryPersonResponse<CategoryPersonResource>> CreateAsync(CreateCategoryPersonResource createCategoryPersonResource)
        {
            // Validate person is existent?
            var tempPerson = await _personRepository.FindByIdAsync(createCategoryPersonResource.PersonId);
            if (tempPerson is null)
                return new CategoryPersonResponse<CategoryPersonResource>($"Person Id '{createCategoryPersonResource.PersonId}' is not existent.");
            // Validate CategoryId is existent?
            var tempCategory = await _categoryRepository.FindByIdAsync(createCategoryPersonResource.CategoryId);
            if (tempCategory is null || tempCategory.Id == createCategoryPersonResource.CategoryId)
                return new CategoryPersonResponse<CategoryPersonResource>($"Category Id '{createCategoryPersonResource.CategoryId}' is invalid.");
            // Validate TechnologyId is existent?
            var listTechnologyBelongToCategoryId = await _technologyRepository.ListAsync(createCategoryPersonResource.CategoryId);
            foreach (var item in listTechnologyBelongToCategoryId)
                if (!createCategoryPersonResource.Technology.Contains(item.Id))
                    return new CategoryPersonResponse<CategoryPersonResource>($"Technology Id '{item.Id}' is not existent.");
            // Remove duplicate element
            createCategoryPersonResource.Technology = createCategoryPersonResource.Technology.RemoveDuplicate();

            // Mapping Resource to CategoryPerson
            var categoryPerson = _mapper.Map<CreateCategoryPersonResource, CategoryPerson>(createCategoryPersonResource);
            categoryPerson.OrderIndex = FindMaximum(createCategoryPersonResource.PersonId);

            try
            {
                await _categoryPersonRepository.AddAsync(categoryPerson);
                await _unitOfWork.CompleteAsync();

                // Mapping Project to Resource
                var resource = _mapper.Map<CategoryPerson, CategoryPersonResource>(categoryPerson);

                return new CategoryPersonResponse<CategoryPersonResource>(resource);
            }
            catch (Exception ex)
            {
                return new CategoryPersonResponse<CategoryPersonResource>($"An error occurred when saving the CategoryPerson: {ex.Message}");
            }
        }

        /// <summary>
        /// Find maximum value of OrderIndex
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private int FindMaximum(int id)
        {
            int maximumOrderIndex =  _categoryPersonRepository.MaximumOrderIndex(id);
            maximumOrderIndex = (maximumOrderIndex <= 0) ? 1 : maximumOrderIndex + 1;

            return maximumOrderIndex;
        }

        public async Task<CategoryPersonResponse<CategoryPersonResource>> UpdateAsync(int id, UpdateCategoryPersonResource updateCategoryPersonResource)
        {
            // Validate Id is existent?
            var tempCategoryPerson = await _categoryPersonRepository.FindByIdAsync(id);
            if (tempCategoryPerson is null)
                return new CategoryPersonResponse<CategoryPersonResource>("CategoryPerson not exist.");
            // Validate CategoryId is existent?
            var tempCategory = await _categoryRepository.FindByIdAsync(updateCategoryPersonResource.CategoryId);
            if (tempCategory is null || tempCategory.Id == updateCategoryPersonResource.CategoryId)
                return new CategoryPersonResponse<CategoryPersonResource>($"Category Id '{updateCategoryPersonResource.CategoryId}' is invalid.");
            // Validate TechnologyId is existent?
            var listTechnologyBelongToCategoryId = await _technologyRepository.ListAsync(updateCategoryPersonResource.CategoryId);
            foreach (var item in listTechnologyBelongToCategoryId)
                if (!updateCategoryPersonResource.Technology.Contains(item.Id))
                    return new CategoryPersonResponse<CategoryPersonResource>($"Technology Id '{item.Id}' is not existent.");
            // Remove duplicate element
            updateCategoryPersonResource.Technology = updateCategoryPersonResource.Technology.RemoveDuplicate();
            // Updating
            _mapper.Map(updateCategoryPersonResource, tempCategoryPerson);

            try
            {
                await _unitOfWork.CompleteAsync();

                // Mapping Project to Resource
                var resource = _mapper.Map<CategoryPerson, CategoryPersonResource>(tempCategoryPerson);

                return new CategoryPersonResponse<CategoryPersonResource>(resource);
            }
            catch (Exception ex)
            {
                return new CategoryPersonResponse<CategoryPersonResource>($"An error occurred when updating the CategoryPerson: {ex.Message}");
            }
        }

        public async Task<CategoryPersonResponse<CategoryPersonResource>> DeleteAsync(int id)
        {
            // Validate Id is existent?
            var tempCategoryPerson = await _categoryPersonRepository.FindByIdAsync(id);
            if (tempCategoryPerson is null)
                return new CategoryPersonResponse<CategoryPersonResource>("CategoryPerson is not existent.");
            // Change property Status: true -> false
            tempCategoryPerson.Status = false;
            
            try
            {
                await _unitOfWork.CompleteAsync();

                // Mapping Project to Resource
                var resource = _mapper.Map<CategoryPerson, CategoryPersonResource>(tempCategoryPerson);

                return new CategoryPersonResponse<CategoryPersonResource>(resource);
            }
            catch (Exception ex)
            {
                return new CategoryPersonResponse<CategoryPersonResource>($"An error occurred when deleting the CategoryPerson: {ex.Message}");
            }
        }

        public async Task<CategoryPersonResponse<CategoryPersonResource>> SwapAsync(SwapResource obj)
        {
            // Validate Id duplicate
            if (obj.CurrentId == obj.TurnedId)
                return new CategoryPersonResponse<CategoryPersonResource>("Current Id/Turned Id is not valid.");
            // Validate Id is existent?
            var currentCategoryPerson = await _categoryPersonRepository.FindByIdAsync(obj.CurrentId);
            var turnedCategoryPerson = await _categoryPersonRepository.FindByIdAsync(obj.TurnedId);
            if (currentCategoryPerson is null || turnedCategoryPerson is null)
                return new CategoryPersonResponse<CategoryPersonResource>("CategoryPerson not exist.");
            if (currentCategoryPerson.PersonId != turnedCategoryPerson.PersonId)
                return new CategoryPersonResponse<CategoryPersonResource>("Current Id/Turned Id is not valid.");
            // Swap property OrderIndex
            int tempOrderIndex = -1;
            tempOrderIndex = currentCategoryPerson.OrderIndex;
            currentCategoryPerson.OrderIndex = turnedCategoryPerson.OrderIndex;
            turnedCategoryPerson.OrderIndex = tempOrderIndex;

            try
            {
                await _unitOfWork.CompleteAsync();

                return new CategoryPersonResponse<CategoryPersonResource>(new CategoryPersonResource());
            }
            catch (Exception ex)
            {
                return new CategoryPersonResponse<CategoryPersonResource>($"An error occurred when swapping the CategoryPerson: {ex.Message}");
            }
        }
    }
}
