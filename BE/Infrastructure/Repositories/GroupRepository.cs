using Business.Domain.Models;
using Business.Domain.Repositories;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GroupRepository : BaseRepository<Group>, IGroupRepository
    {
        #region Constructor
        public GroupRepository(AppDbContext context) : base(context) { }
        #endregion

        #region Method
        
        #endregion
    }
}
