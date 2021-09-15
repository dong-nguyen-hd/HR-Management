#nullable enable
using AutoMapper;
using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using HR_Management.Domain.Services;
using HR_Management.Domain.Services.Communication;
using HR_Management.Extensions;
using HR_Management.Resources;
using HR_Management.Resources.Person;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace HR_Management.Services
{
    public class PersonService : ResponseMessageService, IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PersonService(IPersonRepository personRepository,
            ILocationRepository locationRepository,
            IUriService uriService,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IOptionsSnapshot<ResponseMessage> responseMessage) : base(responseMessage)
        {
            this._personRepository = personRepository;
            this._locationRepository = locationRepository;
            this._uriService = uriService;
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }

        public async Task<PersonResponse<PersonResource>> AssignComponentAsync(int id, ComponentResource component)
        {
            // Validate Id is existent?
            var tempPerson = await _personRepository.FindByIdAsync(id);
            if (tempPerson is null)
                return new PersonResponse<PersonResource>(ResponseMessage.Values["Person_Id_NoData"]);
            try
            {
                tempPerson.OrderIndex = component.OrderIndex.RemoveDuplicate().ConcatenateWithComma();

                await _unitOfWork.CompleteAsync();
                // Mapping
                var resource = _mapper.Map<Person, PersonResource>(tempPerson);

                return new PersonResponse<PersonResource>(resource);
            }
            catch (Exception ex)
            {
                return new PersonResponse<PersonResource>($"{ResponseMessage.Values["Person_Updating_Error"]}: {ex.Message}");
            }
        }

        public async Task<PersonResponse<PersonResource>> CreateAsync(CreatePersonResource createPersonResource, bool isMobile = false)
        {
            // Validate location is existent?
            var tempLocation = !createPersonResource.LocationId.HasValue ? null : await _locationRepository.FindByIdAsync((int)createPersonResource.LocationId);
            if (tempLocation is null)
                createPersonResource.LocationId = null;
            // Mapping Resource to Person
            var person = _mapper.Map<CreatePersonResource, Person>(createPersonResource);

            try
            {
                await _personRepository.AddAsync(person);
                await _unitOfWork.CompleteAsync();

                // Mapping
                var resource = _mapper.Map<Person, PersonResource>(person);
                resource.Avatar = isMobile ?
                    _uriService.GetRouteUri($"{Startup.ImagePathMobile}{person.Avatar}")
                    : _uriService.GetRouteUri($"{Startup.ImagePathWeb}{person.Avatar}");

                return new PersonResponse<PersonResource>(resource);
            }
            catch (Exception ex)
            {
                return new PersonResponse<PersonResource>($"{ResponseMessage.Values["Person_Saving_Error"]}: {ex.Message}");
            }
        }

        public async Task<PersonResponse<PersonResource>> DeleteAsync(int id, bool isMobile = false)
        {
            // Validate Id is existent?
            var tempPerson = await _personRepository.FindByIdAsync(id);
            if (tempPerson is null)
                return new PersonResponse<PersonResource>(ResponseMessage.Values["Person_Id_NoData"]);
            // Change property Status: true -> false
            tempPerson.Status = false;

            try
            {
                await _unitOfWork.CompleteAsync();
                // Mapping
                var resource = _mapper.Map<Person, PersonResource>(tempPerson);
                resource.Avatar = isMobile ?
                    _uriService.GetRouteUri($"{Startup.ImagePathMobile}{tempPerson.Avatar}")
                    : _uriService.GetRouteUri($"{Startup.ImagePathWeb}{tempPerson.Avatar}");

                return new PersonResponse<PersonResource>(resource);
            }
            catch (Exception ex)
            {
                return new PersonResponse<PersonResource>($"{ResponseMessage.Values["Person_Deleting_Error"]}: {ex.Message}");
            }
        }

        public async Task<PersonResponse<PersonResource>> FindByIdAsync(int id, bool isMobile = false)
        {
            var tempPerson = await _personRepository.FindByIdAsync(id);
            if (tempPerson is null)
                return new PersonResponse<PersonResource>(ResponseMessage.Values["Person_Id_NoData"]);
            // Mapping Person to Resource
            var resource = _mapper.Map<Person, PersonResource>(tempPerson);
            resource.Avatar = isMobile ?
                    _uriService.GetRouteUri($"{Startup.ImagePathMobile}{tempPerson.Avatar}")
                    : _uriService.GetRouteUri($"{Startup.ImagePathWeb}{tempPerson.Avatar}");

            return new PersonResponse<PersonResource>(resource);
        }

        public async Task<PersonResponse<PersonResource>> UpdateAsync(int id, UpdatePersonResource updatePersonResource, bool isMobile = false)
        {
            // Validate Id is existent?
            var tempPerson = await _personRepository.FindByIdAsync(id);
            if (tempPerson is null)
                return new PersonResponse<PersonResource>(ResponseMessage.Values["Person_Id_NoData"]);
            // Validate location is existent?
            var tempLocation = !updatePersonResource.LocationId.HasValue ? null : await _locationRepository.FindByIdAsync((int)updatePersonResource.LocationId);
            if (tempLocation is null)
                updatePersonResource.LocationId = null;
            // Mapping Resource to Person
            _mapper.Map(updatePersonResource, tempPerson);

            try
            {
                await _unitOfWork.CompleteAsync();
                // Mapping
                var resource = _mapper.Map<Person, PersonResource>(tempPerson);
                resource.Avatar = isMobile ?
                    _uriService.GetRouteUri($"{Startup.ImagePathMobile}{tempPerson.Avatar}")
                    : _uriService.GetRouteUri($"{Startup.ImagePathWeb}{tempPerson.Avatar}");

                return new PersonResponse<PersonResource>(resource);
            }
            catch (Exception ex)
            {
                return new PersonResponse<PersonResource>($"{ResponseMessage.Values["Person_Updating_Error"]}: {ex.Message}");
            }
        }
    }
}
