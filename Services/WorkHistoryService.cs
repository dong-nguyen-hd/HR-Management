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
    public class WorkHistoryService : IWorkHistoryService
    {
        private readonly IWorkHistoryRepository _workHistoryRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WorkHistoryService(IWorkHistoryRepository workHistoryRepository,
            IPersonRepository personRepository,
            IUnitOfWork unitOfWork)
        {
            this._workHistoryRepository = workHistoryRepository;
            this._personRepository = personRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<WorkHistoryResponse> ListAsync(int personId)
        {
            // Validate person is existent?
            var tempPerson = await _personRepository.FindByIdAsync(personId);
            if (tempPerson is null)
                return new WorkHistoryResponse($"PersonId '{personId}' not exist.");
            // Get list record from DB
            var temp = await _workHistoryRepository.ListAsync(personId);

            return new WorkHistoryResponse(temp);
        }

        public async Task<WorkHistoryResponse> CreateAsync(WorkHistory workHistory)
        {
            // Validate dateTime is valid
            var isSuccess = RelateValidate.ValidateTimeInput(workHistory.StartDate, workHistory.EndDate);
            if (!isSuccess)
                return new WorkHistoryResponse($"StartDate/EndDate is not valid.");
            // Validate person is existent?
            var tempPerson = await _personRepository.FindByIdAsync(workHistory.PersonId);
            if (tempPerson is null)
                return new WorkHistoryResponse($"PersonId '{workHistory.PersonId}' not exist.");
            // Find maximum value of OrderIndex
            int maximumOrderIndex = await _workHistoryRepository.MaximumOrderIndexAsync(workHistory.PersonId);
            maximumOrderIndex = (maximumOrderIndex <= 0) ? 1 : maximumOrderIndex + 1;
            // Assign value
            workHistory.Position = workHistory.Position.RemoveSpaceCharacter();
            workHistory.CompanyName = workHistory.CompanyName.RemoveSpaceCharacter();
            workHistory.OrderIndex = maximumOrderIndex;
            try
            {
                await _workHistoryRepository.AddAsync(workHistory);
                await _unitOfWork.CompleteAsync();

                return new WorkHistoryResponse(workHistory);
            }
            catch (Exception ex)
            {
                return new WorkHistoryResponse($"An error occurred when saving the Work History: {ex.Message}");
            }
        }

        public async Task<WorkHistoryResponse> UpdateAsync(int id, WorkHistory workHistory)
        {
            // Validate dateTime is valid
            var isSuccess = RelateValidate.ValidateTimeInput(workHistory.StartDate, workHistory.EndDate);
            if (!isSuccess)
                return new WorkHistoryResponse($"StartDate/EndDate is not valid.");
            // Validate Id is existent?
            var tempWorkHistory = await _workHistoryRepository.FindByIdAsync(id);
            if (tempWorkHistory is null)
                return new WorkHistoryResponse("Work History not exist.");
            // Update infomation
            tempWorkHistory.Position = workHistory.Position.RemoveSpaceCharacter();
            tempWorkHistory.CompanyName = workHistory.CompanyName.RemoveSpaceCharacter();
            tempWorkHistory.StartDate = workHistory.StartDate;
            tempWorkHistory.EndDate = workHistory.EndDate;
            try
            {
                await _unitOfWork.CompleteAsync();

                return new WorkHistoryResponse(tempWorkHistory);
            }
            catch (Exception ex)
            {
                return new WorkHistoryResponse($"An error occurred when updating the Work History: {ex.Message}");
            }
        }

        public async Task<WorkHistoryResponse> DeleteAsync(int id)
        {
            // Validate Id is existent?
            var tempWorkHistory = await _workHistoryRepository.FindByIdAsync(id);
            if (tempWorkHistory is null)
                return new WorkHistoryResponse("Work History not exist.");
            // Change property Status: true -> false
            tempWorkHistory.Status = false;

            try
            {
                await _unitOfWork.CompleteAsync();

                return new WorkHistoryResponse(tempWorkHistory);
            }
            catch (Exception ex)
            {
                return new WorkHistoryResponse($"An error occurred when deleting the Work History: {ex.Message}");
            }
        }

        public async Task<WorkHistoryResponse> SwapAsync(SwapResource obj)
        {
            // Validate Id duplicate
            if (obj.CurrentId == obj.TurnedId)
                return new WorkHistoryResponse("CurrentId/TurnedId is not valid.");
            // Validate Id is existent?
            var currentWorkHistory = await _workHistoryRepository.FindByIdAsync(obj.CurrentId);
            var turnedWorkHistory = await _workHistoryRepository.FindByIdAsync(obj.TurnedId);
            if (currentWorkHistory is null || turnedWorkHistory is null)
                return new WorkHistoryResponse("Work History not exist.");
            // Swap property OrderIndex
            int tempOrderIndex = -1;
            tempOrderIndex = currentWorkHistory.OrderIndex;
            currentWorkHistory.OrderIndex = turnedWorkHistory.OrderIndex;
            turnedWorkHistory.OrderIndex = tempOrderIndex;

            try
            {
                await _unitOfWork.CompleteAsync();

                return new WorkHistoryResponse(new List<WorkHistory>() { currentWorkHistory, turnedWorkHistory });
            }
            catch (Exception ex)
            {
                return new WorkHistoryResponse($"An error occurred when swapping the Work History: {ex.Message}");
            }
        }
    }
}
