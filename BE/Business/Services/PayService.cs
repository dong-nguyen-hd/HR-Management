using AutoMapper;
using Business.Communication;
using Business.CustomException;
using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Pay;
using Microsoft.Extensions.Options;

namespace Business.Services
{
    public class PayService : BaseService<PayResource, CreatePayResource, UpdatePayResource, Pay>, IPayService
    {
        #region Property
        private readonly IPayRepository _payRepository;
        private readonly ITimesheetRepository _timesheetRepository;
        #endregion

        #region Constructor
        public PayService(IPayRepository payRepository,
            ITimesheetRepository timesheetRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(payRepository, mapper, unitOfWork, responseMessage)
        {
            this._payRepository = payRepository;
            this._timesheetRepository = timesheetRepository;
        }
        #endregion

        #region Method
        public async override Task<BaseResponse<PayResource>> InsertAsync(CreatePayResource createPayResource)
        {
            try
            {
                // Mapping Resource to Pay
                var pay = Mapper.Map<CreatePayResource, Pay>(createPayResource);

                await _payRepository.InsertAsync(pay);
                await UnitOfWork.CompleteAsync();

                return new BaseResponse<PayResource>(true);
            }
            catch (Exception ex)
            {
                throw new MessageResultException(ResponseMessage.Values["Pay_Saving_Error"], ex);
            }
        }

        public async Task<BaseResponse<PayResource>> GetPayByMonthAsync(int personId, DateTime date)
        {
            // Get timesheet belong to person
            var timesheet = await _timesheetRepository.GetTimesheetByPersonIdAsync(personId, date);
            if (timesheet is null)
                return new BaseResponse<PayResource>(ResponseMessage.Values["Timesheet_NoData"]);

            return null;
        }

        #region Private work
       
        #endregion

        #endregion
    }
}
