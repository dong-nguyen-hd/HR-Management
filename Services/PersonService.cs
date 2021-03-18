#nullable enable
using AutoMapper;
using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using HR_Management.Domain.Services;
using HR_Management.Domain.Services.Communication;
using HR_Management.Extensions;
using HR_Management.Resources;
using HR_Management.Resources.Person;
using System;
using System.Threading.Tasks;

namespace HR_Management.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PersonService(IPersonRepository personRepository,
            ILocationRepository locationRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            this._personRepository = personRepository;
            this._locationRepository = locationRepository;
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }

        public async Task<PersonResponse<PersonResource>> AssignComponentAsync(int id, ComponentResource component)
        {
            // Validate Id is existent?
            var tempPerson = await _personRepository.FindByIdAsync(id);
            if (tempPerson is null)
                return new PersonResponse<PersonResource>("Person is not existent.");
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
                return new PersonResponse<PersonResource>($"An error occurred when updating the Person: {ex.Message}");
            }
        }

        public async Task<PersonResponse<PersonResource>> CreateAsync(CreatePersonResource createPersonResource)
        {
            // Validate location is existent?
            var tempLocation = !createPersonResource.LocationId.HasValue ? null : await _locationRepository.FindByIdAsync((int)createPersonResource.LocationId);
            if (tempLocation is null)
                createPersonResource.LocationId = null;
            // Mapping Resource to Person
            var person = _mapper.Map<CreatePersonResource, Person>(createPersonResource);
            // Testing
            person.CreatedBy = "KimYoungKen";

            try
            {
                await _personRepository.AddAsync(person);
                await _unitOfWork.CompleteAsync();

                // Mapping
                var resource = _mapper.Map<Person, PersonResource>(person);

                return new PersonResponse<PersonResource>(resource);
            }
            catch (Exception ex)
            {
                return new PersonResponse<PersonResource>($"An error occurred when saving the Person: {ex.Message}");
            }
        }
            
        public async Task<PersonResponse<PersonResource>> DeleteAsync(int id)
        {
            // Validate Id is existent?
            var tempPerson = await _personRepository.FindByIdAsync(id);
            if (tempPerson is null)
                return new PersonResponse<PersonResource>("Person is not existent.");
            // Change property Status: true -> false
            tempPerson.Status = false;

            try
            {
                await _unitOfWork.CompleteAsync();
                // Mapping
                var resource = _mapper.Map<Person, PersonResource>(tempPerson);

                return new PersonResponse<PersonResource>(resource);
            }
            catch (Exception ex)
            {
                return new PersonResponse<PersonResource>($"An error occurred when deleting the Person: {ex.Message}");
            }
        }

        public async Task<PersonResponse<PersonResource>> FindByIdAsync(int id)
        {
            var tempPerson = await _personRepository.FindByIdAsync(id);
            if (tempPerson is null)
                return new PersonResponse<PersonResource>($"Id '{id}' is not existent.");
            // Mapping Person to Resource
            var resource = _mapper.Map<Person, PersonResource>(tempPerson);

            return new PersonResponse<PersonResource>(resource);
        }

        public async Task<PersonResponse<PersonResource>> UpdateAsync(int id, UpdatePersonResource updatePersonResource)
        {
            // Validate Id is existent?
            var tempPerson = await _personRepository.FindByIdAsync(id);
            if (tempPerson is null)
                return new PersonResponse<PersonResource>("Person is not existent.");
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

                return new PersonResponse<PersonResource>(resource);
            }
            catch (Exception ex)
            {
                return new PersonResponse<PersonResource>($"An error occurred when updating the Person: {ex.Message}");
            }
        }
    }
}
