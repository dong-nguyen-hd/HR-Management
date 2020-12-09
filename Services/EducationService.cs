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

        public EducationService(IEducationRespository educationRespository)
        {
            this._educationRespository = educationRespository;
        }

        public async Task<IEnumerable<Education>> ListAsync()
        {
            return await _educationRespository.ListAsync();
        }
    }
}
