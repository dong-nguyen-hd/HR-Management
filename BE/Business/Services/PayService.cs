using AutoMapper;
using Business.Communication;
using Business.CustomException;
using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Pay;
using Business.Resources.Timesheet;
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
                var workDayResource = await _timesheetRepository.GetTotalWorkDayAsync(createPayResource.PersonId, createPayResource.Date);
                if (workDayResource is null)
                    return new BaseResponse<PayResource>(ResponseMessage.Values["Timesheet_NoData"]);

                // Mapping Resource to Pay
                var pay = Mapper.Map<CreatePayResource, Pay>(createPayResource);
                var payResource = CalculateSalary(pay, workDayResource);

                await _payRepository.InsertAsync(pay);
                await UnitOfWork.CompleteAsync();

                return new BaseResponse<PayResource>(payResource);
            }
            catch (Exception ex)
            {
                throw new MessageResultException(ResponseMessage.Values["Pay_Saving_Error"], ex);
            }
        }

        #region Private work
        private PayResource CalculateSalary(Pay src, WorkDayResource workDayResource)
        {
            src.WorkDay = workDayResource.WorkDay;
            src.TotalWorkDay = workDayResource.TotalWorkDay;

            decimal grossWithoutBonus = (decimal)workDayResource.WorkDay * src.BaseSalary / (decimal)workDayResource.TotalWorkDay;
            Receivables receivables = new(grossWithoutBonus);

            src.PIT = receivables.PIT;
            src.HealthInsurance = receivables.HealthInsurance;
            src.SocialInsurance = receivables.SocialInsurance;

            var payResource = Mapper.Map<Pay, PayResource>(src);

            payResource.PITPercent = receivables.PIT;
            payResource.HealthInsurancePercent = receivables.HealthInsurance;
            payResource.SocialInsurancePercent = receivables.SocialInsurance;

            payResource.SocialInsurance = grossWithoutBonus / 100 * (decimal)receivables.SocialInsurance;
            payResource.HealthInsurance = grossWithoutBonus / 100 * (decimal)receivables.HealthInsurance;
            payResource.PIT = grossWithoutBonus / 100 * (decimal)receivables.PIT;

            payResource.Gross = grossWithoutBonus + src.Bonus + src.Allowance;
            payResource.NET = (payResource.SocialInsurance + payResource.HealthInsurance + payResource.PIT) * grossWithoutBonus + src.Bonus + src.Allowance;

            return payResource;
        }
        #endregion

        #endregion
    }
}
