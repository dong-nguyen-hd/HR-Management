#nullable enable
using AutoMapper;
using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using HR_Management.Domain.Services;
using HR_Management.Domain.Services.Communication;
using HR_Management.Extensions;
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

            // Mapping
            List<ProjectResource> tempResource = new List<ProjectResource>();
            foreach (var item in tempProject)
            {
                var temp = _mapper.Map<Project, ProjectResource>(item);

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

            return new ProjectResponse<IEnumerable<ProjectResource>>(tempResource);
        }

        public async Task<ProjectResponse<ProjectResource>> CreateAsync(CreateProjectResource createProjectResource)
        {
            // Validate dateTime is valid
            var isSuccess = RelateValidate.ValidateTimeInput(createProjectResource.StartDate, createProjectResource.EndDate);
            if (!isSuccess)
                return new ProjectResponse<ProjectResource>($"Start Date/End Date is not valid.");
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
            // Find maximum value of OrderIndex
            int maximumOrderIndex = await _projectRepository.MaximumOrderIndexAsync(createProjectResource.PersonId);
            maximumOrderIndex = (maximumOrderIndex <= 0) ? 1 : maximumOrderIndex + 1;

            // Mapping Resource to Project
            var project = _mapper.Map<CreateProjectResource, Project>(createProjectResource);
            // Assign value
            project.Technology = createProjectResource.Technology.ConcatenateWithComma();
            project.OrderIndex = maximumOrderIndex;
            // Mapping Project to Resource
            var resource = _mapper.Map<Project, ProjectResource>(project);

            // Mapping
            // Mapping technology into Dictionary type
            foreach (var technology in createProjectResource.Technology)
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
                await _projectRepository.AddAsync(project);
                await _unitOfWork.CompleteAsync();
                /// Assign Id value while added
                resource.Id = project.Id;

                return new ProjectResponse<ProjectResource>(resource);
            }
            catch (Exception ex)
            {
                return new ProjectResponse<ProjectResource>($"An error occurred when saving the Project: {ex.Message}");
            }
        }

        public async Task<ProjectResponse<ProjectResource>> UpdateAsync(int id, UpdateProjectResource updateProjectResource)
        {
            // Validate dateTime is valid
            var isSuccess = RelateValidate.ValidateTimeInput(updateProjectResource.StartDate, updateProjectResource.EndDate);
            if (!isSuccess)
                return new ProjectResponse<ProjectResource>($"Start Date/End Date is not valid.");
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
            // Update infomation
            tempProject.Name = updateProjectResource.Name.RemoveSpaceCharacter();
            tempProject.Description = updateProjectResource.Description.RemoveSpaceCharacter();
            tempProject.Position = updateProjectResource.Position.RemoveSpaceCharacter();
            tempProject.Responsibilities = updateProjectResource.Responsibilities.RemoveSpaceCharacter();
            tempProject.TeamSize = updateProjectResource.TeamSize;
            tempProject.Technology = updateProjectResource.Technology.ConcatenateWithComma();
            tempProject.StartDate = updateProjectResource.StartDate;
            tempProject.EndDate = updateProjectResource.EndDate;

            // Mapping
            // Mapping Project to Resource
            var resource = _mapper.Map<Project, ProjectResource>(tempProject);
            // Mapping technology into Dictionary type
            foreach (var technology in updateProjectResource.Technology)
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
            // Mapping
            // Mapping Project to Resource
            var resource = _mapper.Map<Project, ProjectResource>(tempProject);
            // Mapping technology into Dictionary type
            string[] listTechnology = tempProject.Technology.Split(',');
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
