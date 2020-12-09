using HR_Management.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Management.Domain.Services
{
    public interface IEducationService
    {
        Task<IEnumerable<Education>> ListAsync();
    }
}
