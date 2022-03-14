using AutoMapper;
using Business.Communication;
using Business.CustomException;
using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Office;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Business.Services
{
    public class OfficeService : BaseService<OfficeResource, CreateOfficeResource, UpdateOfficeResource, Office>, IOfficeService
    {
        #region Property
        private readonly IOfficeRepository _officeRepository;
        #endregion

        #region Constructor
        public OfficeService(IOfficeRepository officeRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(officeRepository, mapper, unitOfWork, responseMessage)
        {
            this._officeRepository = officeRepository;
        }
        #endregion

        #region Method
        public override async Task<BaseResponse<OfficeResource>> InsertAsync(CreateOfficeResource createLocationResource)
        {
            try
            {
                // Mapping Resource to Office
                var office = Mapper.Map<CreateOfficeResource, Office>(createLocationResource);

                await _officeRepository.InsertAsync(office);
                await UnitOfWork.CompleteAsync();

                return new BaseResponse<OfficeResource>(Mapper.Map<Office, OfficeResource>(office));
            }
            catch (Exception ex)
            {
                throw new MessageResultException(ResponseMessage.Values["Office_Saving_Error"], ex);
            }
        }
        #endregion
    }
}
