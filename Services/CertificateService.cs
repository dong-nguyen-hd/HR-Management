#nullable enable
using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using HR_Management.Domain.Services;
using HR_Management.Domain.Services.Communication;
using HR_Management.Extensions;
using HR_Management.Resources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HR_Management.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly ICertificateRepository _certificateRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CertificateService(ICertificateRepository certificateRepository,
            IPersonRepository personRepository,
            IUnitOfWork unitOfWork)
        {
            this._certificateRepository = certificateRepository;
            this._personRepository = personRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CertificateResponse> ListAsync(int personId)
        {
            // Validate person is existent?
            var tempPerson = await _personRepository.FindByIdAsync(personId);
            if (tempPerson is null)
                return new CertificateResponse($"PersonId '{personId}' not exist.");
            // Get list record from DB
            var temp = await _certificateRepository.ListAsync(personId);

            return new CertificateResponse(temp);
        }

        public async Task<CertificateResponse> CreateAsync(Certificate certificate)
        {
            // Validate dateTime is valid
            var isSuccess = RelateValidate.ValidateTimeInput(certificate.StartDate, certificate.EndDate);
            if (!isSuccess)
                return new CertificateResponse($"StartDate/EndDate is not valid.");
            // Validate person is existent?
            var tempPerson = await _personRepository.FindByIdAsync(certificate.PersonId);
            if (tempPerson is null)
                return new CertificateResponse($"PersonId '{certificate.PersonId}' not exist.");
            // Find maximum value of OrderIndex
            int maximumOrderIndex = await _certificateRepository.MaximumOrderIndexAsync(certificate.PersonId);
            maximumOrderIndex = (maximumOrderIndex <= 0) ? 1 : maximumOrderIndex + 1;
            // Assign value
            certificate.Name = certificate.Name.RemoveSpaceCharacter();
            certificate.Provider = certificate.Provider.RemoveSpaceCharacter();
            certificate.OrderIndex = maximumOrderIndex;
            try
            {
                await _certificateRepository.AddAsync(certificate);
                await _unitOfWork.CompleteAsync();

                return new CertificateResponse(certificate);
            }
            catch (Exception ex)
            {
                return new CertificateResponse($"An error occurred when saving the certificate: {ex.Message}");
            }
        }

        public async Task<CertificateResponse> UpdateAsync(int id, Certificate certificate)
        {
            // Validate dateTime is valid
            var isSuccess = RelateValidate.ValidateTimeInput(certificate.StartDate, certificate.EndDate);
            if (!isSuccess)
                return new CertificateResponse($"StartDate/EndDate is not valid.");
            // Validate Id is existent?
            var tempCertificate = await _certificateRepository.FindByIdAsync(id);
            if (tempCertificate is null)
                return new CertificateResponse("Certificate not exist.");
            // Update infomation
            tempCertificate.Name = certificate.Name.RemoveSpaceCharacter();
            tempCertificate.Provider = certificate.Provider.RemoveSpaceCharacter();
            tempCertificate.StartDate = certificate.StartDate;
            tempCertificate.EndDate = certificate.EndDate;
            try
            {
                await _unitOfWork.CompleteAsync();

                return new CertificateResponse(tempCertificate);
            }
            catch (Exception ex)
            {
                return new CertificateResponse($"An error occurred when updating the certificate: {ex.Message}");
            }
        }

        public async Task<CertificateResponse> DeleteAsync(int id)
        {
            // Validate Id is existent?
            var tempCertificate = await _certificateRepository.FindByIdAsync(id);
            if (tempCertificate is null)
                return new CertificateResponse("Certificate not exist.");
            // Change property Status: true -> false
            tempCertificate.Status = false;

            try
            {
                await _unitOfWork.CompleteAsync();

                return new CertificateResponse(tempCertificate);
            }
            catch (Exception ex)
            {
                return new CertificateResponse($"An error occurred when deleting the certificate: {ex.Message}");
            }
        }

        public async Task<CertificateResponse> SwapAsync(SwapResource obj)
        {
            // Validate Id duplicate
            if (obj.CurrentId == obj.TurnedId)
                return new CertificateResponse("CurrentId/TurnedId is not valid.");
            // Validate Id is existent?
            var currentCertificate = await _certificateRepository.FindByIdAsync(obj.CurrentId);
            var turnedCertificate = await _certificateRepository.FindByIdAsync(obj.TurnedId);
            if (currentCertificate is null || turnedCertificate is null)
                return new CertificateResponse("Certificate not exist.");
            // Swap property OrderIndex
            int tempOrderIndex = -1;
            tempOrderIndex = currentCertificate.OrderIndex;
            currentCertificate.OrderIndex = turnedCertificate.OrderIndex;
            turnedCertificate.OrderIndex = tempOrderIndex;

            try
            {
                await _unitOfWork.CompleteAsync();

                return new CertificateResponse(new List<Certificate>() { currentCertificate, turnedCertificate });
            }
            catch (Exception ex)
            {
                return new CertificateResponse($"An error occurred when swapping the certificate: {ex.Message}");
            }
        }
    }
}
