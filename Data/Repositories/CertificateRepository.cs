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
    public class CertificateRepository : BaseRepository, ICertificateRepository
    {
        public CertificateRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Certificate>> ListAsync(int personId)
        {
            var temp = await _context.Certificates.Where(x => x.PersonId == personId && x.Status)
                .OrderBy(x => x.OrderIndex)
                .ThenBy(x => x.EndDate)
                .ToListAsync();

            return temp;
        }

        public async Task AddAsync(Certificate certificate)
        {
            await _context.Certificates.AddAsync(certificate);
        }

        public void Update(Certificate certificate)
        {
            _context.Certificates.Update(certificate);
        }

        public async Task<Certificate> FindByIdAsync(int id)
        {
            var temp = await _context.Certificates.Where(x => x.Id == id && x.Status).FirstOrDefaultAsync();

            return temp;
        }

        public void Remove(Certificate certificate)
        {
            _context.Certificates.Remove(certificate);
        }

        public async Task<int> MaximumOrderIndexAsync(int personId)
        {
            var tempList = await _context.Certificates.Where(x => x.PersonId == personId && x.Status).ToListAsync();
            int maximum = (tempList.Count == 0) ? 0 : tempList.Max(x => x.OrderIndex);
            
            return maximum;
        }
    }
}
