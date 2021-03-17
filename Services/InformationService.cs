#nullable enable
using AutoMapper;
using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using HR_Management.Domain.Services;
using HR_Management.Domain.Services.Communication;
using HR_Management.Extensions;
using HR_Management.Resources.Information;
using HR_Management.Resources.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Services
{
    public class InformationService : IInformationService
    {
        
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;
        private readonly IPersonRepository _personRepository;
        
        public InformationService(IPersonRepository personRepository,
            IUriService uriService,
            IMapper mapper
            )
        {
            this._uriService = uriService;
            this._mapper = mapper;
            this._personRepository = personRepository;
        }
        
        public async Task<InformationResponse<IEnumerable<InformationResource>>> ListWithPaginationAsync(QueryResource pagintation, string route)
        {
            var tempPerson = await _personRepository.ListPaginationAsync(pagintation);
            var totalRecords = await _personRepository.TotalRecordAsync();
            // Mapping
            var tempResource = _mapper.Map<IEnumerable<Person>, IEnumerable<InformationResource>>(tempPerson);

            var resource = new InformationResponse<IEnumerable<InformationResource>>(tempResource);
            // Using extension-method for pagination
            resource.CreatePaginationResponse<InformationResponse<IEnumerable<InformationResource>>, IEnumerable<InformationResource>>(pagintation, totalRecords, _uriService, route);
            
            return resource;
        }
    }
}
