using AutoMapper;
using Business.Communication;
using Business.CustomException;
using Business.Data;
using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Extensions;
using Business.Resources;
using Business.Resources.Account;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class AccountService : BaseService<AccountResource, CreateAccountResource, UpdateAccountResource, Account>, IAccountService
    {
        #region Property
        private readonly IAccountRepository _accountRepository;
        #endregion

        #region Constructor
        public AccountService(IAccountRepository accountRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(accountRepository, mapper, unitOfWork, responseMessage)
        {
            this._accountRepository = accountRepository;
        }
        #endregion

        #region Method
        public async Task<PaginationResponse<IEnumerable<AccountResource>>> ListPaginationAsync(QueryResource pagintation)
        {
            var tempAccount = await _accountRepository.ListPaginationAsync(pagintation);
            var totalRecords = await _accountRepository.TotalRecordAsync();

            // Mapping
            var tempResource = Mapper.Map<IEnumerable<Account>, IEnumerable<AccountResource>>(tempAccount);

            var resource = new PaginationResponse<IEnumerable<AccountResource>>(tempResource);
            // Using extension-method for pagination
            resource.CreatePaginationResponse(pagintation, totalRecords);

            return resource;
        }

        public async Task<BaseResponse<AccountResource>> SelfUpdateAsync(int id, SelfUpdateAccountResource resource)
        {
            var tempAccount = await _accountRepository.GetByIdAsync(id);

            // Update infomation
            Mapper.Map(resource, tempAccount);

            try
            {
                await UnitOfWork.CompleteAsync();
                // Mapping
                var result = Mapper.Map<AccountResource>(tempAccount);

                return new BaseResponse<AccountResource>(result);
            }
            catch (Exception ex)
            {
                throw new MessageResultException(ResponseMessage.Values["Account_Updating_Error"], ex);
            }
        }

        public async Task<BaseResponse<AccountResource>> UpdatePasswordAsync(int id, UpdatePasswordAccountResource resource)
        {
            // Validate Id is existent?
            var tempAccount = await _accountRepository.GetByIdAsync(id);
            if(!tempAccount.Password.CheckingPassword(resource.OldPassword))
                return new BaseResponse<AccountResource>(ResponseMessage.Values["NoData"]);

            // Update infomation
            tempAccount.Password = resource.NewPassword.HashingPassword(Constant.IterationCount);
            tempAccount.LastActivity = DateTime.UtcNow;

            try
            {
                await UnitOfWork.CompleteAsync();
                // Mapping
                var result = Mapper.Map<AccountResource>(tempAccount);

                return new BaseResponse<AccountResource>(result);
            }
            catch (Exception ex)
            {
                throw new MessageResultException(ResponseMessage.Values["Account_Updating_Error"], ex);
            }
        }
        #endregion
    }
}
