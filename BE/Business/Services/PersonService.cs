using AutoMapper;
using Business.Communication;
using Business.Domain.Models;
using Business.Domain.Repositories;
using Business.Domain.Services;
using Business.Extensions;
using Business.Resources;
using Business.Resources.Person;
using Business.Resources.Technology;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class PersonService : BaseService<PersonResource, CreatePersonResource, UpdatePersonResource, Person>, IPersonService
    {
        #region Constructor
        public PersonService(IPersonRepository personRepository,
            ITechnologyService technologyService,
            ILocationRepository locationRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IOptionsMonitor<ResponseMessage> responseMessage) : base(personRepository, mapper, unitOfWork, responseMessage)
        {
            this._personRepository = personRepository;
            this._locationRepository = locationRepository;
            this._technologyService = technologyService;
        }
        #endregion

        #region Property
        private readonly IPersonRepository _personRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ITechnologyService _technologyService;
        #endregion

        #region Method
        public override async Task<BaseResponse<PersonResource>> InsertAsync(CreatePersonResource createPersonResource)
        {
            // Validate Location is existent?
            var tempLocation = !createPersonResource.LocationId.HasValue ? null : await _locationRepository.GetByIdAsync((int)createPersonResource.LocationId);
            if (tempLocation is null)
                createPersonResource.LocationId = null;
            // Mapping Resource to Person
            var person = Mapper.Map<CreatePersonResource, Person>(createPersonResource);

            try
            {
                await _personRepository.InsertAsync(person);
                await UnitOfWork.CompleteAsync();

                return new BaseResponse<PersonResource>(Mapper.Map<Person, PersonResource>(person));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BaseResponse<PersonResource>(ResponseMessage.Values["Person_Saving_Error"]);
            }
        }

        public override async Task<BaseResponse<PersonResource>> UpdateAsync(int id, UpdatePersonResource updatePersonResource)
        {
            // Validate Id is existent?
            var tempPerson = await _personRepository.GetByIdAsync(id);
            if(tempPerson is null)
                return new BaseResponse<PersonResource>(ResponseMessage.Values["Person_Id_NoData"]);

            // Validate LocationId is existent?
            var tempLocation = !updatePersonResource.LocationId.HasValue ? null : await _locationRepository.GetByIdAsync((int)updatePersonResource.LocationId);
            if (tempLocation is null)
                updatePersonResource.LocationId = null;

            // Mapping Resource to Person
            Mapper.Map(updatePersonResource, tempPerson);

            try
            {
                await UnitOfWork.CompleteAsync();

                return new BaseResponse<PersonResource>(Mapper.Map<Person, PersonResource>(tempPerson));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BaseResponse<PersonResource>(ResponseMessage.Values["Person_Saving_Error"]);
            }
        }

        public async Task<BaseResponse<PersonResource>> AssignComponentAsync(int id, ComponentResource component)
        {
            // Validate Id is existent?
            var tempPerson = await _personRepository.GetByIdAsync(id);
            if (tempPerson is null)
                return new BaseResponse<PersonResource>(ResponseMessage.Values["Person_Id_NoData"]);

            try
            {
                tempPerson.OrderIndex = component.OrderIndex.RemoveDuplicate().ConcatenateWithComma();

                await UnitOfWork.CompleteAsync();
                // Mapping
                var resource = Mapper.Map<Person, PersonResource>(tempPerson);

                return new BaseResponse<PersonResource>(resource);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BaseResponse<PersonResource>(ResponseMessage.Values["Person_Updating_Error"]);
            }
        }

        public async Task<PaginationResponse<IEnumerable<PersonResource>>> GetPaginationAsync(QueryResource pagination, int? locationId = null)
        {
            var totalTechnology = await _technologyService.GetAllAsync();
            var paginationPerson = await _personRepository.GetPaginationAsync(pagination, locationId);
            var totalRecords = await _personRepository.TotalRecordAsync();

            // Mapping
            var tempResource = ConvertPersonResource(totalTechnology.Resource, paginationPerson);

            var resource = new PaginationResponse<IEnumerable<PersonResource>>(tempResource);

            // Using extension-method for pagination
            resource.CreatePaginationResponse(pagination, totalRecords);

            return resource;
        }

        private IEnumerable<PersonResource> ConvertPersonResource(IEnumerable<TechnologyResource> totalTechnology, IEnumerable<Person> totalPerson)
        {
            List<PersonResource> listPersonResource = new List<PersonResource>(totalPerson.Count());

            foreach (var person in totalPerson)
            {
                var tempPersonResource = Mapper.Map<Person, PersonResource>(person);

                // Project mapping
                var listProject = person.Projects.ToList();
                var countProject = listProject.Count;
                for (int i = 0; i < countProject; i++)
                    if (!string.IsNullOrEmpty(listProject?[i].Technology))
                        tempPersonResource.Project[i].Technology = totalTechnology.IntersectTechnology(listProject[i].Technology);

                // Category-Person mapping
                var listCategoryPerson = person.CategoryPersons.ToList();
                var countCategoryPerson = listCategoryPerson.Count;
                for (int i = 0; i < countCategoryPerson; i++)
                    if (!string.IsNullOrEmpty(listCategoryPerson?[i].Technology))
                        tempPersonResource.CategoryPerson[i].Technology = totalTechnology.IntersectTechnology(listCategoryPerson[i].Technology);

                listPersonResource.Add(tempPersonResource);
            }

            return listPersonResource;
        }
        #endregion
    }
}
