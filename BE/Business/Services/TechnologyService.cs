using AutoMapper;
using Business.Communication;
using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Resources;
using Business.Resources.Technology;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{
    public class TechnologyService : BaseService<TechnologyResource, CreateTechnologyResource, UpdateTechnologyResource, Technology>, ITechnologyService
    {
        #region Constructor
        public TechnologyService(ITechnologyRepository technologyRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(technologyRepository, mapper, unitOfWork, responseMessage)
        {
            this._technologyRepository = technologyRepository;
        }
        #endregion

        #region Property
        private readonly ITechnologyRepository _technologyRepository;
        #endregion

        #region Method
        public async Task<BaseResponse<IEnumerable<TechnologyResource>>> GetByCategoryAsync(int categoryId)
        {
            // Get list record from DB
            var tempTechnology = await _technologyRepository.GetByCategoryAsync(categoryId);
            // Mapping Technology to TechnologyResource
            var result = Mapper.Map<IEnumerable<Technology>, IEnumerable<TechnologyResource>>(tempTechnology);

            return new BaseResponse<IEnumerable<TechnologyResource>>(result);
        }
        #endregion
    }
}
