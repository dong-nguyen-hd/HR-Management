#nullable enable
using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using HR_Management.Domain.Services;
using HR_Management.Domain.Services.Communication;
using HR_Management.Extensions;
using HR_Management.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Services
{
    public class EducationService : IEducationService
    {
        private readonly IEducationRepository _educationRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EducationService(IEducationRepository educationRepository,
            IPersonRepository personRepository,
            IUnitOfWork unitOfWork)
        {
            this._educationRepository = educationRepository;
            this._personRepository = personRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<EducationResponse> ListAsync(int personId)
        {
            // Validate person is existent?
            var tempPerson = await _personRepository.FindByIdAsync(personId);
            if (tempPerson is null)
                return new EducationResponse($"PersonId '{personId}' not exist.");
            // Get list record from DB
            var temp = await _educationRepository.ListAsync(personId);

            return new EducationResponse(temp);
        }

        public async Task<EducationResponse> CreateAsync(Education education)
        {
            // Validate dateTime is valid
            var isSuccess = RelateValidate.ValidateTimeInput(education.StartDate, education.EndDate);
            if (!isSuccess)
                return new EducationResponse($"StartDate/EndDate is not valid.");
            // Validate person is existent?
            var tempPerson = await _personRepository.FindByIdAsync(education.PersonId);
            if (tempPerson is null)
                return new EducationResponse($"PersonId '{education.PersonId}' not exist.");
            // Find maximum value of OrderIndex
            int maximumOrderIndex = await _educationRepository.MaximumOrderIndexAsync(education.PersonId);
            maximumOrderIndex = (maximumOrderIndex <= 0) ? 1 : maximumOrderIndex + 1;
            // Assign value
            education.CollegeName = education.CollegeName.RemoveSpaceCharacter();
            education.Major = education.Major.RemoveSpaceCharacter();
            education.OrderIndex = maximumOrderIndex;
            try
            {
                await _educationRepository.AddAsync(education);
                await _unitOfWork.CompleteAsync();

                return new EducationResponse(education);
            }
            catch (Exception ex)
            {
                return new EducationResponse($"An error occurred when saving the education: {ex.Message}");
            }
        }

        public async Task<EducationResponse> UpdateAsync(int id, Education education)
        {
            // Validate dateTime is valid
            var isSuccess = RelateValidate.ValidateTimeInput(education.StartDate, education.EndDate);
            if (!isSuccess)
                return new EducationResponse($"StartDate/EndDate is not valid.");
            // Validate Id is existent?
            var tempEducation = await _educationRepository.FindByIdAsync(id);
            if (tempEducation is null)
                return new EducationResponse("Education not exist.");
            // Update infomation
            tempEducation.CollegeName = education.CollegeName.RemoveSpaceCharacter();
            tempEducation.Major = education.Major.RemoveSpaceCharacter();
            tempEducation.StartDate = education.StartDate;
            tempEducation.EndDate = education.EndDate;
            try
            {
                await _unitOfWork.CompleteAsync();

                return new EducationResponse(tempEducation);
            }
            catch (Exception ex)
            {
                return new EducationResponse($"An error occurred when updating the education: {ex.Message}");
            }
        }

        public async Task<EducationResponse> DeleteAsync(int id)
        {
            // Validate Id is existent?
            var tempEducation = await _educationRepository.FindByIdAsync(id);
            if (tempEducation is null)
                return new EducationResponse("Education not exist.");
            // Change property Status: true -> false
            tempEducation.Status = false;
            
            try
            {
                await _unitOfWork.CompleteAsync();

                return new EducationResponse(tempEducation);
            }
            catch (Exception ex)
            {
                return new EducationResponse($"An error occurred when deleting the education: {ex.Message}");
            }
        }

        public async Task<EducationResponse> SwapAsync(SwapResource obj)
        {
            // Validate Id duplicate
            if (obj.CurrentId == obj.TurnedId)
                return new EducationResponse("CurrentId/TurnedId is not valid.");
            // Validate Id is existent?
            var currentEducation = await _educationRepository.FindByIdAsync(obj.CurrentId);
            var turnedEducation = await _educationRepository.FindByIdAsync(obj.TurnedId);
            if (currentEducation is null || turnedEducation is null)
                return new EducationResponse("Education not exist.");
            // Swap property OrderIndex
            int tempOrderIndex = -1;
            tempOrderIndex = currentEducation.OrderIndex;
            currentEducation.OrderIndex = turnedEducation.OrderIndex;
            turnedEducation.OrderIndex = tempOrderIndex;

            try
            {
                await _unitOfWork.CompleteAsync();

                return new EducationResponse(new List<Education>(){ currentEducation, turnedEducation });
            }
            catch (Exception ex)
            {
                return new EducationResponse($"An error occurred when swapping the education: {ex.Message}");
            }
        }
    }
}
