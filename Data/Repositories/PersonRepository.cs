#nullable enable
using HR_Management.Data.Contexts;
using HR_Management.Domain.Models;
using HR_Management.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HR_Management.Data.Repositories
{
    public class PersonRepository : BaseRepository, IPersonRepository
    {
        public PersonRepository(AppDbContext context) :base(context) { }

        public Task AddAsync(Person person)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Person> FindByIdAsync(int id)
        {
            var temp = await _context.People.FirstOrDefaultAsync(x => x.Id == id && x.Status);

            return temp;
        }

        public Task<IEnumerable<Person>> ListAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(Person person)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Person person)
        {
            throw new System.NotImplementedException();
        }
    }
}
