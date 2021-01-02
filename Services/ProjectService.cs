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
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IProjectRepository projectRepository,
            IPersonRepository personRepository,
            IUnitOfWork unitOfWork)
        {
            this._projectRepository = projectRepository;
            this._personRepository = personRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProjectResponse> ListAsync(int personId)
        {
            // Validate person is existent?
            var tempPerson = await _personRepository.FindByIdAsync(personId);
            if (tempPerson is null)
                return new ProjectResponse($"PersonId '{personId}' not exist.");
            // Get list record from DB
            var temp = await _projectRepository.ListAsync(personId);

            return new ProjectResponse(temp);
        }

        public async Task<ProjectResponse> CreateAsync(Project project)
        {
            // Validate dateTime is valid
            var isSuccess = RelateValidate.ValidateTimeInput(project.StartDate, project.EndDate);
            if (!isSuccess)
                return new ProjectResponse($"StartDate/EndDate is not valid.");
            // Validate person is existent?
            var tempPerson = await _personRepository.FindByIdAsync(project.PersonId);
            if (tempPerson is null)
                return new ProjectResponse($"PersonId '{project.PersonId}' not exist.");
            // Find maximum value of OrderIndex
            int maximumOrderIndex = await _projectRepository.MaximumOrderIndexAsync(project.PersonId);
            maximumOrderIndex = (maximumOrderIndex <= 0) ? 1 : maximumOrderIndex + 1;
            // Assign value
            project.Name = project.Name.RemoveSpaceCharacter();
            project.Description = project.Description.RemoveSpaceCharacter();
            project.Position = project.Position.RemoveSpaceCharacter();
            project.Responsibilities = project.Responsibilities.RemoveSpaceCharacter();
            project.Technology = project.Technology.RemoveSpaceCharacter();
            project.OrderIndex = maximumOrderIndex;

            try
            {
                await _projectRepository.AddAsync(project);
                await _unitOfWork.CompleteAsync();

                return new ProjectResponse(project);
            }
            catch (Exception ex)
            {
                return new ProjectResponse($"An error occurred when saving the project: {ex.Message}");
            }
        }

        public async Task<ProjectResponse> UpdateAsync(int id, Project project)
        {
            // Validate dateTime is valid
            var isSuccess = RelateValidate.ValidateTimeInput(project.StartDate, project.EndDate);
            if (!isSuccess)
                return new ProjectResponse($"StartDate/EndDate is not valid.");
            // Validate Id is existent?
            var tempProject = await _projectRepository.FindByIdAsync(id);
            if (tempProject is null)
                return new ProjectResponse("Project not exist.");
            // Update infomation
            tempProject.Name = project.Name.RemoveSpaceCharacter();
            tempProject.Description = project.Description.RemoveSpaceCharacter();
            tempProject.Position = project.Position.RemoveSpaceCharacter();
            tempProject.Responsibilities = project.Responsibilities.RemoveSpaceCharacter();
            tempProject.Technology = project.Technology.RemoveSpaceCharacter();
            tempProject.StartDate = project.StartDate;
            tempProject.EndDate = project.EndDate;

            try
            {
                await _unitOfWork.CompleteAsync();

                return new ProjectResponse(tempProject);
            }
            catch (Exception ex)
            {
                return new ProjectResponse($"An error occurred when updating the project: {ex.Message}");
            }
        }

        public async Task<ProjectResponse> DeleteAsync(int id)
        {
            // Validate Id is existent?
            var tempProject = await _projectRepository.FindByIdAsync(id);
            if (tempProject is null)
                return new ProjectResponse("Project not exist.");
            // Change property Status: true -> false
            tempProject.Status = false;

            try
            {
                await _unitOfWork.CompleteAsync();

                return new ProjectResponse(tempProject);
            }
            catch (Exception ex)
            {
                return new ProjectResponse($"An error occurred when deleting the project: {ex.Message}");
            }
        }

        public async Task<ProjectResponse> SwapAsync(SwapResource obj)
        {
            // Validate Id duplicate
            if (obj.CurrentId == obj.TurnedId)
                return new ProjectResponse("CurrentId/TurnedId is not valid.");
            // Validate Id is existent?
            var currentProject = await _projectRepository.FindByIdAsync(obj.CurrentId);
            var turnedProject = await _projectRepository.FindByIdAsync(obj.TurnedId);
            if (currentProject is null || turnedProject is null)
                return new ProjectResponse("Project not exist.");
            // Swap property OrderIndex
            int tempOrderIndex = -1;
            tempOrderIndex = currentProject.OrderIndex;
            currentProject.OrderIndex = turnedProject.OrderIndex;
            turnedProject.OrderIndex = tempOrderIndex;

            try
            {
                await _unitOfWork.CompleteAsync();

                return new ProjectResponse(new List<Project>() { currentProject, turnedProject });
            }
            catch (Exception ex)
            {
                return new ProjectResponse($"An error occurred when swapping the project: {ex.Message}");
            }
        }
    }
}
