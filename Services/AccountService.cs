#nullable enable
using AutoMapper;
using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using HR_Management.Domain.Services;
using HR_Management.Domain.Services.Communication;
using HR_Management.Extensions;
using HR_Management.Resources.Account;
using HR_Management.Resources.Queries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IAccountRepository accountRepository,
            IUriService uriService,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            this._accountRepository = accountRepository;
            this._uriService = uriService;
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }

        public async Task<AccountResponse<AccountResource>> CreateAsync(CreateAccountResource createAccountResource)
        {
            // Validate User Name is existent?
            var isValid = await _accountRepository.ValidateUserNameAsync(createAccountResource.UserName);
            if (!isValid)
                return new AccountResponse<AccountResource>("User name is existent.");
            // Mapping Resource to Account
            var account = _mapper.Map<CreateAccountResource, Account>(createAccountResource);

            try
            {
                await _accountRepository.AddAsync(account);
                await _unitOfWork.CompleteAsync();

                // Mapping
                var resource = _mapper.Map<Account, AccountResource>(account);

                return new AccountResponse<AccountResource>(resource);
            }
            catch (Exception ex)
            {
                return new AccountResponse<AccountResource>($"An error occurred when saving the Account: {ex.Message}");
            }
        }

        public async Task<AccountResponse<AccountResource>> DeleteAsync(int id)
        {
            // Validate Id is existent?
            var tempAccount = await _accountRepository.FindByIdAsync(id);
            if (tempAccount is null)
                return new AccountResponse<AccountResource>("Account is not existent.");
            // Change property Status: true -> false
            tempAccount.Status = false;

            try
            {
                await _unitOfWork.CompleteAsync();
                // Mapping
                var resource = _mapper.Map<Account, AccountResource>(tempAccount);

                return new AccountResponse<AccountResource>(resource);
            }
            catch (Exception ex)
            {
                return new AccountResponse<AccountResource>($"An error occurred when deleting the Account: {ex.Message}");
            }
        }

        public async Task<AccountResponse<AccountResource>> FindByIdAsync(int id)
        {
            var tempAccount = await _accountRepository.FindByIdAsync(id);
            if (tempAccount is null)
                return new AccountResponse<AccountResource>($"Id '{id}' is not existent.");
            // Mapping Person to Resource
            var resource = _mapper.Map<Account, AccountResource>(tempAccount);

            return new AccountResponse<AccountResource>(resource);
        }

        public async Task<AccountResponse<IEnumerable<AccountResource>>> ListWithPaginationAsync(QueryResource pagintation, string route)
        {
            var tempAccount = await _accountRepository.ListPaginationAsync(pagintation);
            var totalRecords = await _accountRepository.TotalRecordAsync();
            // Mapping
            var tempResource = _mapper.Map<IEnumerable<Account>, IEnumerable<AccountResource>>(tempAccount);

            var resource = new AccountResponse<IEnumerable<AccountResource>>(tempResource);
            // Using extension-method for pagination
            resource.CreatePaginationResponse<AccountResponse<IEnumerable<AccountResource>>, IEnumerable<AccountResource>>(pagintation, totalRecords, _uriService, route);

            return resource;
        }

        public async Task<AccountResponse<AccountResource>> UpdateAsync(int id, UpdateAccountResource updateAccountResource)
        {
            // Validate Id is existent?
            var tempAccount = await _accountRepository.FindByIdAsync(id);
            if (tempAccount is null)
                return new AccountResponse<AccountResource>("Account is not existent.");

            // Mapping Resource to Person
            _mapper.Map(updateAccountResource, tempAccount);

            try
            {
                await _unitOfWork.CompleteAsync();
                // Mapping
                var resource = _mapper.Map<Account, AccountResource>(tempAccount);

                return new AccountResponse<AccountResource>(resource);
            }
            catch (Exception ex)
            {
                return new AccountResponse<AccountResource>($"An error occurred when updating the Account: {ex.Message}");
            }
        }
    }
}
