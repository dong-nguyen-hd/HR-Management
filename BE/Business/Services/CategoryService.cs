using AutoMapper;
using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Category;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Business.Services
{
    public class CategoryService : BaseService<CategoryResource, CreateCategoryResource, UpdateCategoryResource, Category>, ICategoryService
    {
        #region Constructor
        public CategoryService(ICategoryRepository categoryRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<CategoryService> logger,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(categoryRepository, mapper, unitOfWork, logger, responseMessage)
        {
        }
        #endregion
    }
}
