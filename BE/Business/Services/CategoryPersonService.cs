using AutoMapper;
using Business.Communication;
using Business.CustomException;
using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.CategoryPerson;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CategoryPersonService : BaseService<CategoryPersonResource, CreateCategoryPersonResource, UpdateCategoryPersonResource, CategoryPerson>, ICategoryPersonService
    {
        #region Property
        private readonly ICategoryPersonRepository _categoryPersonRepository;
        #endregion

        #region Constructor
        public CategoryPersonService(ICategoryPersonRepository categoryPersonRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(categoryPersonRepository, mapper, unitOfWork, responseMessage)
        {
            this._categoryPersonRepository = categoryPersonRepository;
        }
        #endregion

        #region Method
        public override async Task<BaseResponse<CategoryPersonResource>> InsertAsync(CreateCategoryPersonResource createCategoryPersonResource)
        {
            try
            {
                // Validate CategoryPerson
                if (await _categoryPersonRepository.ValidateExistent(createCategoryPersonResource.PersonId, createCategoryPersonResource.CategoryId))
                    return new BaseResponse<CategoryPersonResource>(ResponseMessage.Values["CategoryPerson_Id_Invalid"]);

                var tempCategoryPerson = Mapper.Map<CreateCategoryPersonResource, CategoryPerson>(createCategoryPersonResource);
                tempCategoryPerson.OrderIndex = await _categoryPersonRepository.MaximumOrderIndexAsync(createCategoryPersonResource.PersonId);

                await _categoryPersonRepository.InsertAsync(tempCategoryPerson);
                await UnitOfWork.CompleteAsync();

                return new BaseResponse<CategoryPersonResource>(Mapper.Map<CategoryPerson, CategoryPersonResource>(tempCategoryPerson));
            }
            catch (Exception ex)
            {
                throw new MessageResultException(ResponseMessage.Values["CategoryPerson_Saving_Error"], ex);
            }
        }
        #endregion
    }
}
