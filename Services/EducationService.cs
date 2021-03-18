#nullable enable
using AutoMapper;
using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using HR_Management.Domain.Services;
using HR_Management.Domain.Services.Communication;
using HR_Management.Resources;
using HR_Management.Resources.Education;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Services
{
    public class EducationService : IEducationService
    {
        private readonly IEducationRepository _educationRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public EducationService(IEducationRepository educationRepository,
            IPersonRepository personRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            this._educationRepository = educationRepository;
            this._personRepository = personRepository;
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }

        public async Task<EducationResponse<IEnumerable<EducationResource>>> ListAsync(int personId)
        {
            // Validate person is existent?
            var tempPerson = await _personRepository.FindByIdAsync(personId);
            if (tempPerson is null)
                return new EducationResponse<IEnumerable<EducationResource>>($"Person Id '{personId}' is not existent.");
            // Get list record from DB
            var tempEducation = await _educationRepository.ListAsync(personId);
            // Mapping Project to Resource
            var resource = _mapper.Map<IEnumerable<Education>, IEnumerable<EducationResource>>(tempEducation);

            return new EducationResponse<IEnumerable<EducationResource>>(resource);
        }

        public async Task<EducationResponse<EducationResource>> CreateAsync(CreateEducationResource createEducationResource)
        {
            // Validate person is existent?
            var tempPerson = await _personRepository.FindByIdAsync(createEducationResource.PersonId);
            if (tempPerson is null)
                return new EducationResponse<EducationResource>($"Person Id '{createEducationResource.PersonId}' is not existent.");

            // Mapping Resource to Project
            var education = _mapper.Map<CreateEducationResource, Education>(createEducationResource);
            education.OrderIndex = FindMaximum(education.PersonId);

            try
            {
                await _educationRepository.AddAsync(education);
                await _unitOfWork.CompleteAsync();
                // Mapping
                var resource = _mapper.Map<Education, EducationResource>(education);

                return new EducationResponse<EducationResource>(resource);
            }
            catch (Exception ex)
            {
                return new EducationResponse<EducationResource>($"An error occurred when saving the Education: {ex.Message}");
            }
        }

        /// <summary>
        /// Find maximum value of OrderIndex
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private int FindMaximum(int id)
        {
            int maximumOrderIndex = _educationRepository.MaximumOrderIndex(id);
            maximumOrderIndex = (maximumOrderIndex <= 0) ? 1 : maximumOrderIndex + 1;

            return maximumOrderIndex;
        }

        public async Task<EducationResponse<EducationResource>> UpdateAsync(int id, UpdateEducationResource updateEducationResource)
        {
            // Validate Id is existent?
            var tempEducation = await _educationRepository.FindByIdAsync(id);
            if (tempEducation is null)
                return new EducationResponse<EducationResource>("Education is not existent.");
            // Update infomation
            _mapper.Map(updateEducationResource, tempEducation);

            try
            {
                await _unitOfWork.CompleteAsync();
                // Mapping
                var resource = _mapper.Map<Education, EducationResource>(tempEducation);

                return new EducationResponse<EducationResource>(resource);
            }
            catch (Exception ex)
            {
                return new EducationResponse<EducationResource>($"An error occurred when updating the Education: {ex.Message}");
            }
        }

        public async Task<EducationResponse<EducationResource>> DeleteAsync(int id)
        {
            // Validate Id is existent?
            var tempEducation = await _educationRepository.FindByIdAsync(id);
            if (tempEducation is null)
                return new EducationResponse<EducationResource>("Education is not existent.");
            // Change property Status: true -> false
            tempEducation.Status = false;

            try
            {
                await _unitOfWork.CompleteAsync();
                // Mapping
                var resource = _mapper.Map<Education, EducationResource>(tempEducation);

                return new EducationResponse<EducationResource>(resource);
            }
            catch (Exception ex)
            {
                return new EducationResponse<EducationResource>($"An error occurred when deleting the Education: {ex.Message}");
            }
        }

        public async Task<EducationResponse<EducationResource>> SwapAsync(SwapResource obj)
        {
            // Validate Id duplicate
            if (obj.CurrentId == obj.TurnedId)
                return new EducationResponse<EducationResource>("Current Id/Turned Id is not valid.");
            // Validate Id is existent?
            var currentEducation = await _educationRepository.FindByIdAsync(obj.CurrentId);
            var turnedEducation = await _educationRepository.FindByIdAsync(obj.TurnedId);
            if (currentEducation is null || turnedEducation is null)
                return new EducationResponse<EducationResource>("Education is not existent.");
            if (currentEducation.PersonId != turnedEducation.PersonId)
                return new EducationResponse<EducationResource>("Current Id/Turned Id is not valid.");
            // Swap property OrderIndex
            int tempOrderIndex = -1;
            tempOrderIndex = currentEducation.OrderIndex;
            currentEducation.OrderIndex = turnedEducation.OrderIndex;
            turnedEducation.OrderIndex = tempOrderIndex;

            try
            {
                await _unitOfWork.CompleteAsync();

                return new EducationResponse<EducationResource>(new EducationResource());
            }
            catch (Exception ex)
            {
                return new EducationResponse<EducationResource>($"An error occurred when swapping the Education: {ex.Message}");
            }
        }
    }
}
