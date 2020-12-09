using HR_Management.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Management.Domain.Repositories
{
    public interface IEducationRespository
    {
        Task<IEnumerable<Education>> ListAsync();
    }
}
