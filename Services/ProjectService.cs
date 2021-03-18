#nullable enable
using AutoMapper;
using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using HR_Management.Domain.Services;
using HR_Management.Domain.Services.Communication;
using HR_Management.Resources;
using HR_Management.Resources.Project;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITechnologyRepository _technologyRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IProjectRepository projectRepository,
            ITechnologyRepository technologyRepository,
            IPersonRepository personRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            this._projectRepository = projectRepository;
            this._technologyRepository = technologyRepository;
            this._personRepository = personRepository;
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }

        public async Task<ProjectResponse<IEnumerable<ProjectResource>>> ListAsync(int personId)
        {
            // Validate person is existent?
            var tempPerson = await _personRepository.FindByIdAsync(personId);
            if (tempPerson is null)
                return new ProjectResponse<IEnumerable<ProjectResource>>($"Person Id '{personId}' is not existent.");

            // Get list record from DB
            var tempProject = await _projectRepository.ListAsync(personId);

            // Mapping Project to Resource
            var resource = _mapper.Map<IEnumerable<Project>, IEnumerable<ProjectResource>>(tempProject);

            return new ProjectResponse<IEnumerable<ProjectResource>>(resource);
        }

        public async Task<ProjectResponse<ProjectResource>> CreateAsync(CreateProjectResource createProjectResource)
        {
            // Validate person is existent?
            var tempPerson = await _personRepository.FindByIdAsync(createProjectResource.PersonId);
            if (tempPerson is null)
                return new ProjectResponse<ProjectResource>($"Person Id '{createProjectResource.PersonId}' is not existent.");
            // Validate TechnologyId is existent?
            foreach (var item in createProjectResource.Technology)
            {
                var temp = await this._technologyRepository.FindByIdAsync(item);
                if (temp is null)
                    return new ProjectResponse<ProjectResource>($"Technology Id is not existent.");
            }

            // Mapping Resource to Project
            var project = _mapper.Map<CreateProjectResource, Project>(createProjectResource);
            project.OrderIndex = FindMaximum(project.PersonId);
            
            try
            {
                await _projectRepository.AddAsync(project);
                await _unitOfWork.CompleteAsync();
                
                // Mapping Project to Resource
                var resource = _mapper.Map<Project, ProjectResource>(project);

                return new ProjectResponse<ProjectResource>(resource);
            }
            catch (Exception ex)
            {
                return new ProjectResponse<ProjectResource>($"An error occurred when saving the Project: {ex.Message}");
            }
        }

        /// <summary>
        /// Find maximum value of OrderIndex
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private int FindMaximum(int id)
        {
            int maximumOrderIndex = _projectRepository.MaximumOrderIndex(id);
            maximumOrderIndex = (maximumOrderIndex <= 0) ? 1 : maximumOrderIndex + 1;

            return maximumOrderIndex;
        }

        public async Task<ProjectResponse<ProjectResource>> UpdateAsync(int id, UpdateProjectResource updateProjectResource)
        {
            // Validate Id is existent?
            var tempProject = await _projectRepository.FindByIdAsync(id);
            if (tempProject is null)
                return new ProjectResponse<ProjectResource>("Project is not existent.");
            // Validate TechnologyId is existent?
            foreach (var item in updateProjectResource.Technology)
            {
                var temp = await this._technologyRepository.FindByIdAsync(item);
                if (temp is null)
                    return new ProjectResponse<ProjectResource>($"Technology Id is not existent.");
            }
            // Updating
            _mapper.Map(updateProjectResource, tempProject);

            try
            {
                await _unitOfWork.CompleteAsync();

                // Mapping Project to Resource
                var resource = _mapper.Map<Project, ProjectResource>(tempProject);

                return new ProjectResponse<ProjectResource>(resource);
            }
            catch (Exception ex)
            {
                return new ProjectResponse<ProjectResource>($"An error occurred when updating the Project: {ex.Message}");
            }
        }

        public async Task<ProjectResponse<ProjectResource>> DeleteAsync(int id)
        {
            // Validate Id is existent?
            var tempProject = await _projectRepository.FindByIdAsync(id);
            if (tempProject is null)
                return new ProjectResponse<ProjectResource>("Project is not existent.");
            // Change property Status: true -> false
            tempProject.Status = false;

            try
            {
                await _unitOfWork.CompleteAsync();

                // Mapping Project to Resource
                var resource = _mapper.Map<Project, ProjectResource>(tempProject);

                return new ProjectResponse<ProjectResource>(resource);
            }
            catch (Exception ex)
            {
                return new ProjectResponse<ProjectResource>($"An error occurred when deleting the Project: {ex.Message}");
            }
        }

        public async Task<ProjectResponse<ProjectResource>> SwapAsync(SwapResource obj)
        {
            // Validate Id duplicate
            if (obj.CurrentId == obj.TurnedId)
                return new ProjectResponse<ProjectResource>("Current Id/Turned Id is not valid.");
            // Validate Id is existent?
            var currentProject = await _projectRepository.FindByIdAsync(obj.CurrentId);
            var turnedProject = await _projectRepository.FindByIdAsync(obj.TurnedId);
            if (currentProject is null || turnedProject is null)
                return new ProjectResponse<ProjectResource>("Project is not existent.");
            if (currentProject.PersonId != turnedProject.PersonId)
                return new ProjectResponse<ProjectResource>("Current Id/Turned Id is not valid.");
            // Swap property OrderIndex
            int tempOrderIndex = -1;
            tempOrderIndex = currentProject.OrderIndex;
            currentProject.OrderIndex = turnedProject.OrderIndex;
            turnedProject.OrderIndex = tempOrderIndex;

            try
            {
                await _unitOfWork.CompleteAsync();

                return new ProjectResponse<ProjectResource>(new ProjectResource());
            }
            catch (Exception ex)
            {
                return new ProjectResponse<ProjectResource>($"An error occurred when swapping the Project: {ex.Message}");
            }
        }
    }
}
