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
        private readonly ITechnologyRepository _technologyRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryPersonService(ICategoryPersonRepository categoryPersonRepository,
            ITechnologyRepository technologyRepository,
            IPersonRepository personRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            this._categoryPersonRepository = categoryPersonRepository;
            this._technologyRepository = technologyRepository;
            this._personRepository = personRepository;
            this._mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryPersonResponse<IEnumerable<CategoryPersonResource>>> ListAsync(int personId)
        {
            // Validate person is existent?
            var tempPerson = await _personRepository.FindByIdAsync(personId);
            if (tempPerson is null)
                return new CategoryPersonResponse<IEnumerable<CategoryPersonResource>>($"Person Id '{personId}' is not existent.");
            // Get list record from DB
            var tempCategoryPerson = await _categoryPersonRepository.ListAsync(personId);

            // Mapping
            List<CategoryPersonResource> tempResource = new List<CategoryPersonResource>();
            foreach (var item in tempCategoryPerson)
            {
                var temp = _mapper.Map<CategoryPerson, CategoryPersonResource>(item);

                // Mapping technology into Dictionary type
                string[] listTechnology = item.Technology.Split(',');
                foreach (var technology in listTechnology)
                {
                    var specific = await this._technologyRepository.FindByIdAsync(Convert.ToInt32(technology.Trim()));
                    if (specific is null) continue;

                    temp.Technology.Add(new Dictionary<int, string>()
                    {
                        { specific.Id, specific.Name}
                    });
                }

                tempResource.Add(temp);
            }

            return new CategoryPersonResponse<IEnumerable<CategoryPersonResource>>(tempResource);
        }

        public async Task<CategoryPersonResponse<CategoryPersonResource>> CreateAsync(CreateCategoryPersonResource createCategoryPersonResource)
        {
            // Validate person is existent?
            var tempPerson = await _personRepository.FindByIdAsync(createCategoryPersonResource.PersonId);
            if (tempPerson is null)
                return new CategoryPersonResponse<CategoryPersonResource>($"Person Id '{createCategoryPersonResource.PersonId}' is not existent.");

            var listTechnologyBelongToCategoryId = await this._technologyRepository.ListAsync(createCategoryPersonResource.CategoryId);
            // Validate CategoryId is existent?
            if (!listTechnologyBelongToCategoryId.GetEnumerator().MoveNext())
                return new CategoryPersonResponse<CategoryPersonResource>($"Category Id '{createCategoryPersonResource.CategoryId}' is not existent.");
            // Validate TechnologyId is existent?
            foreach (var item in listTechnologyBelongToCategoryId)
            {
                if (!createCategoryPersonResource.Technology.Contains(item.Id))
                    return new CategoryPersonResponse<CategoryPersonResource>($"Technology Id is not existent.");
            }
            // Find maximum value of OrderIndex
            int maximumOrderIndex = await _categoryPersonRepository.MaximumOrderIndexAsync(createCategoryPersonResource.PersonId);
            maximumOrderIndex = (maximumOrderIndex <= 0) ? 1 : maximumOrderIndex + 1;

            // Mapping Resource to CategoryPerson
            var categoryPerson = _mapper.Map<CreateCategoryPersonResource, CategoryPerson>(createCategoryPersonResource);
            // Assign value
            categoryPerson.Technology = createCategoryPersonResource.Technology.ConcatenateWithComma();
            categoryPerson.OrderIndex = maximumOrderIndex;
            // Mapping CategoryPerson to Resource
            var resource = _mapper.Map<CategoryPerson, CategoryPersonResource>(categoryPerson);

            // Mapping
            // Mapping technology into Dictionary type
            foreach (var technology in createCategoryPersonResource.Technology)
            {
                var specific = await this._technologyRepository.FindByIdAsync(technology);
                if (specific is null) continue;

                resource.Technology.Add(new Dictionary<int, string>()
                {
                    { specific.Id, specific.Name}
                });
            }

            try
            {
                await _categoryPersonRepository.AddAsync(categoryPerson);
                await _unitOfWork.CompleteAsync();
                /// Assign Id value while added
                resource.Id = categoryPerson.Id;

                return new CategoryPersonResponse<CategoryPersonResource>(resource);
            }
            catch (Exception ex)
            {
                return new CategoryPersonResponse<CategoryPersonResource>($"An error occurred when saving the CategoryPerson: {ex.Message}");
            }
        }

        public async Task<CategoryPersonResponse<CategoryPersonResource>> UpdateAsync(int id, UpdateCategoryPersonResource updateCategoryPersonResource)
        {
            // Validate Id is existent?
            var tempCategoryPerson = await _categoryPersonRepository.FindByIdAsync(id);
            if (tempCategoryPerson is null)
                return new CategoryPersonResponse<CategoryPersonResource>("CategoryPerson not exist.");

            var listTechnologyBelongToCategoryId = await this._technologyRepository.ListAsync(updateCategoryPersonResource.CategoryId);
            // Validate CategoryId is existent?
            if (!listTechnologyBelongToCategoryId.GetEnumerator().MoveNext())
                return new CategoryPersonResponse<CategoryPersonResource>($"Category Id '{updateCategoryPersonResource.CategoryId}' is not existent.");
            // Validate TechnologyId is existent?
            foreach (var item in listTechnologyBelongToCategoryId)
            {
                if (!updateCategoryPersonResource.Technology.Contains(item.Id))
                    return new CategoryPersonResponse<CategoryPersonResource>($"Technology Id is not existent.");
            }
            // Update infomation
            tempCategoryPerson.Technology = updateCategoryPersonResource.Technology.ConcatenateWithComma();
            tempCategoryPerson.CategoryId = updateCategoryPersonResource.CategoryId;

            // Mapping
            // Mapping CategoryPerson to Resource
            var resource = _mapper.Map<CategoryPerson, CategoryPersonResource>(tempCategoryPerson);
            // Mapping technology into Dictionary type
            foreach (var technology in updateCategoryPersonResource.Technology)
            {
                var specific = await this._technologyRepository.FindByIdAsync(technology);
                if (specific is null) continue;

                resource.Technology.Add(new Dictionary<int, string>()
                {
                    { specific.Id, specific.Name}
                });
            }

            try
            {
                await _unitOfWork.CompleteAsync();

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
            // Mapping
            // Mapping CategoryPerson to Resource
            var resource = _mapper.Map<CategoryPerson, CategoryPersonResource>(tempCategoryPerson);
            // Mapping technology into Dictionary type
            string[] listTechnology = tempCategoryPerson.Technology.Split(',');
            foreach (var technology in listTechnology)
            {
                var specific = await this._technologyRepository.FindByIdAsync(Convert.ToInt32(technology.Trim()));
                if (specific is null) continue;

                resource.Technology.Add(new Dictionary<int, string>()
                {
                    { specific.Id, specific.Name }
                });
            }

            try
            {
                await _unitOfWork.CompleteAsync();

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
