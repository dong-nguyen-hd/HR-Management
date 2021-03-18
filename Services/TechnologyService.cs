#nullable enable
using AutoMapper;
using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using HR_Management.Domain.Services;
using HR_Management.Domain.Services.Communication;
using HR_Management.Resources.Technology;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Services
{
    public class TechnologyService : ITechnologyService
    {
        private readonly ITechnologyRepository _technologyRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TechnologyService(ITechnologyRepository technologyRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            this._technologyRepository = technologyRepository;
            this._categoryRepository = categoryRepository;
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }

        public async Task<TechnologyResponse<IEnumerable<TechnologyResource>>> ListAsync()
        {
            // Get list record from DB
            var tempTechnology = await _technologyRepository.ListAsync();
            // Mapping Technology to Resource
            var resource = _mapper.Map<IEnumerable<Technology>, IEnumerable<TechnologyResource>>(tempTechnology);

            return new TechnologyResponse<IEnumerable<TechnologyResource>>(resource);
        }

        public async Task<TechnologyResponse<IEnumerable<TechnologyResource>>> ListAsync(int categoryId)
        {
            // Validate category is existent?
            var tempPerson = await _categoryRepository.FindByIdAsync(categoryId);
            if (tempPerson is null)
                return new TechnologyResponse<IEnumerable<TechnologyResource>>($"CategoryId '{categoryId}' is not existent.");
            // Get list record from DB
            var tempTechnology = await _technologyRepository.ListAsync(categoryId);
            // Mapping Technology to Resource
            var resource = _mapper.Map<IEnumerable<Technology>, IEnumerable<TechnologyResource>>(tempTechnology);

            return new TechnologyResponse<IEnumerable<TechnologyResource>>(resource);
        }

        public async Task<TechnologyResponse<TechnologyResource>> CreateAsync(CreateTechnologyResource createTechnologyResource)
        {
            // Validate CategoryId is existent?
            var tempPerson = await _categoryRepository.FindByIdAsync(createTechnologyResource.CategoryId);
            if (tempPerson is null)
                return new TechnologyResponse<TechnologyResource>($"CategoryId '{createTechnologyResource.CategoryId}' is not existent.");
            // Mapping Resource to Technology
            var technology = _mapper.Map<CreateTechnologyResource, Technology>(createTechnologyResource);

            try
            {
                await _technologyRepository.AddAsync(technology);
                await _unitOfWork.CompleteAsync();
                // Mapping
                var resource = _mapper.Map<Technology, TechnologyResource>(technology);

                return new TechnologyResponse<TechnologyResource>(resource);
            }
            catch (Exception ex)
            {
                return new TechnologyResponse<TechnologyResource>($"An error occurred when saving the Technology: {ex.Message}");
            }
        }

        public async Task<TechnologyResponse<TechnologyResource>> UpdateAsync(int id, UpdateTechnologyResource updateTechnologyResource)
        {
            // Validate Id is existent?
            var tempTechnology = await _technologyRepository.FindByIdAsync(id);
            if (tempTechnology is null)
                return new TechnologyResponse<TechnologyResource>("Technology is not existent.");
            // Updating
            _mapper.Map(updateTechnologyResource, tempTechnology);
            
            try
            {
                await _unitOfWork.CompleteAsync();
                // Mapping
                var resource = _mapper.Map<Technology, TechnologyResource>(tempTechnology);

                return new TechnologyResponse<TechnologyResource>(resource);
            }
            catch (Exception ex)
            {
                return new TechnologyResponse<TechnologyResource>($"An error occurred when updating the Technology: {ex.Message}");
            }
        }

        public async Task<TechnologyResponse<TechnologyResource>> DeleteAsync(int id)
        {
            // Validate Id is existent?
            var tempTechnology = await _technologyRepository.FindByIdAsync(id);
            if (tempTechnology is null)
                return new TechnologyResponse<TechnologyResource>("Technology is not existent.");
            // Change property Status: true -> false
            tempTechnology.Status = false;

            try
            {
                await _unitOfWork.CompleteAsync();
                // Mapping
                var resource = _mapper.Map<Technology, TechnologyResource>(tempTechnology);

                return new TechnologyResponse<TechnologyResource>(resource);
            }
            catch (Exception ex)
            {
                return new TechnologyResponse<TechnologyResource>($"An error occurred when deleting the Technology: {ex.Message}");
            }
        }

        public async Task<List<TechnologyResource>> ConvertListAsync(string technology)
        {
            List<TechnologyResource> listResource = new List<TechnologyResource>();

            string[] listTechnology = technology.Split(',');
            foreach (var item in listTechnology)
            {
                var temp = await _technologyRepository.FindByIdAsync(Convert.ToInt32(item.Trim()));
                if (temp is null) continue;
                listResource.Add(_mapper.Map<Technology, TechnologyResource>(temp));
            }

            return listResource;
        }
    }
}
