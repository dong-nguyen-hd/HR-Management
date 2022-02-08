using AutoMapper;
using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Project;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Business.Services
{
    public class ProjectService : BaseService<ProjectResource, CreateProjectResource, UpdateProjectResource, Project>, IProjectService
    {
        #region Constructor
        public ProjectService(IProjectRepository projectRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<ProjectService> logger,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(projectRepository, mapper, unitOfWork, logger, responseMessage)
        {
        }
        #endregion
    }
}
