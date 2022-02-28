using AutoMapper;
using Business.Communication;
using Business.CustomException;
using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Category;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CategoryService : BaseService<CategoryResource, CreateCategoryResource, UpdateCategoryResource, Category>, ICategoryService
    {
        #region Property
        private readonly ICategoryRepository _categoryRepository;
        #endregion

        #region Constructor
        public CategoryService(ICategoryRepository categoryRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(categoryRepository, mapper, unitOfWork, responseMessage)
        {
            this._categoryRepository = categoryRepository;
        }
        #endregion

        #region Method
        public override async Task<BaseResponse<CategoryResource>> InsertAsync(CreateCategoryResource createCategoryResource)
        {
            try
            {
                var tempCategory = Mapper.Map<CreateCategoryResource, Category>(createCategoryResource);

                await _categoryRepository.InsertAsync(tempCategory);
                await UnitOfWork.CompleteAsync();

                return new BaseResponse<CategoryResource>(Mapper.Map<Category, CategoryResource>(tempCategory));
            }
            catch (Exception ex)
            {
                throw new MessageResultException(ResponseMessage.Values["Category_Saving_Error"], ex);
            }
        }
        #endregion
    }
}
