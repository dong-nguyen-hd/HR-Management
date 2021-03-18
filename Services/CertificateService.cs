#nullable enable
using AutoMapper;
using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using HR_Management.Domain.Services;
using HR_Management.Domain.Services.Communication;
using HR_Management.Extensions;
using HR_Management.Resources;
using HR_Management.Resources.Certificate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly ICertificateRepository _certificateRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CertificateService(ICertificateRepository certificateRepository,
            IPersonRepository personRepository,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            this._certificateRepository = certificateRepository;
            this._personRepository = personRepository;
            this._mapper = mapper;
            this._unitOfWork = unitOfWork;
        }

        public async Task<CertificateResponse<IEnumerable<CertificateResource>>> ListAsync(int personId)
        {
            // Validate person is existent?
            var tempPerson = await _personRepository.FindByIdAsync(personId);
            if (tempPerson is null)
                return new CertificateResponse<IEnumerable<CertificateResource>>($"Person Id '{personId}' is not existent.");
            // Get list record from DB
            var tempCertificate = await _certificateRepository.ListAsync(personId);
            // Mapping Project to Resource
            var resource = _mapper.Map<IEnumerable<Certificate>, IEnumerable<CertificateResource>>(tempCertificate);

            return new CertificateResponse<IEnumerable<CertificateResource>>(resource);
        }

        public async Task<CertificateResponse<CertificateResource>> CreateAsync(CreateCertificateResource createCertificateResource)
        {
            // Validate person is existent?
            var tempPerson = await _personRepository.FindByIdAsync(createCertificateResource.PersonId);
            if (tempPerson is null)
                return new CertificateResponse<CertificateResource>($"Person Id '{createCertificateResource.PersonId}' is not existent.");
            
            // Mapping Resource to Project
            var certificate = _mapper.Map<CreateCertificateResource, Certificate>(createCertificateResource);
            certificate.OrderIndex = FindMaximum(certificate.PersonId);

            try
            {
                await _certificateRepository.AddAsync(certificate);
                await _unitOfWork.CompleteAsync();
                // Mapping
                var resource = _mapper.Map<Certificate, CertificateResource>(certificate);

                return new CertificateResponse<CertificateResource>(resource);
            }
            catch (Exception ex)
            {
                return new CertificateResponse<CertificateResource>($"An error occurred when saving the Certificate: {ex.Message}");
            }
        }

        /// <summary>
        /// Find maximum value of OrderIndex
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private int FindMaximum(int id)
        {
            int maximumOrderIndex = _certificateRepository.MaximumOrderIndex(id);
            maximumOrderIndex = (maximumOrderIndex <= 0) ? 1 : maximumOrderIndex + 1;

            return maximumOrderIndex;
        }

        public async Task<CertificateResponse<CertificateResource>> UpdateAsync(int id, UpdateCertificateResource updateCertificateResource)
        {
            // Validate Id is existent?
            var tempCertificate = await _certificateRepository.FindByIdAsync(id);
            if (tempCertificate is null)
                return new CertificateResponse<CertificateResource>("Certificate is not existent.");
            // Mapping Resource to Certificate
            _mapper.Map(updateCertificateResource, tempCertificate);

            try
            {
                await _unitOfWork.CompleteAsync();
                // Mapping
                var resource = _mapper.Map<Certificate, CertificateResource>(tempCertificate);

                return new CertificateResponse<CertificateResource>(resource);
            }
            catch (Exception ex)
            {
                return new CertificateResponse<CertificateResource>($"An error occurred when updating the Certificate: {ex.Message}");
            }
        }

        public async Task<CertificateResponse<CertificateResource>> DeleteAsync(int id)
        {
            // Validate Id is existent?
            var tempCertificate = await _certificateRepository.FindByIdAsync(id);
            if (tempCertificate is null)
                return new CertificateResponse<CertificateResource>("Certificate is not existent.");
            // Change property Status: true -> false
            tempCertificate.Status = false;

            try
            {
                await _unitOfWork.CompleteAsync();
                // Mapping
                var resource = _mapper.Map<Certificate, CertificateResource>(tempCertificate);

                return new CertificateResponse<CertificateResource>(resource);
            }
            catch (Exception ex)
            {
                return new CertificateResponse<CertificateResource>($"An error occurred when deleting the Certificate: {ex.Message}");
            }
        }

        public async Task<CertificateResponse<CertificateResource>> SwapAsync(SwapResource obj)
        {
            // Validate Id duplicate
            if (obj.CurrentId == obj.TurnedId)
                return new CertificateResponse<CertificateResource>("Current Id/Turned Id is not valid.");
            // Validate Id is existent?
            var currentCertificate = await _certificateRepository.FindByIdAsync(obj.CurrentId);
            var turnedCertificate = await _certificateRepository.FindByIdAsync(obj.TurnedId);
            if (currentCertificate is null || turnedCertificate is null)
                return new CertificateResponse<CertificateResource>("Certificate is not existent.");
            if (currentCertificate.PersonId != turnedCertificate.PersonId)
                return new CertificateResponse<CertificateResource>("Current Id/Turned Id is not valid.");
            // Swap property OrderIndex
            int tempOrderIndex = -1;
            tempOrderIndex = currentCertificate.OrderIndex;
            currentCertificate.OrderIndex = turnedCertificate.OrderIndex;
            turnedCertificate.OrderIndex = tempOrderIndex;

            try
            {
                await _unitOfWork.CompleteAsync();

                return new CertificateResponse<CertificateResource>(new CertificateResource());
            }
            catch (Exception ex)
            {
                return new CertificateResponse<CertificateResource>($"An error occurred when swapping the Certificate: {ex.Message}");
            }
        }
    }
}
