using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using HR_Management.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Management.Services
{
    public class EducationService : IEducationService
    {
        private readonly IEducationRespository _educationRespository;
        private readonly IUnitOfWork _unitOfWork;

        public EducationService(IEducationRespository educationRespository, IUnitOfWork unitOfWork)
        {
            this._educationRespository = educationRespository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Education>> ListAsync(int personId)
        {
            return await _educationRespository.ListAsync(personId);
        }
    }
}
