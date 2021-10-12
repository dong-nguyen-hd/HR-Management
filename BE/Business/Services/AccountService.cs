using AutoMapper;
using Business.Communication;
using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Extensions;
using Business.Resources;
using Business.Resources.Account;
using Microsoft.Extensions.Options;
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
        public async Task<PaginationResponse<IEnumerable<AccountResource>>> ListPaginationAsync(QueryResource pagintation, string route)
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
        #endregion
    }
}
