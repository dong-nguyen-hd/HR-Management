using AutoMapper;
using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.WorkHistory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Business.Services
{
    public class WorkHistoryService : BaseService<WorkHistoryResource, CreateWorkHistoryResource, UpdateWorkHistoryResource, WorkHistory>, IWorkHistoryService
    {
        #region Constructor
        public WorkHistoryService(IWorkHistoryRepository workHistoryRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<WorkHistoryService> logger,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(workHistoryRepository, mapper, unitOfWork, logger, responseMessage)
        {
        }
        #endregion
    }
}
