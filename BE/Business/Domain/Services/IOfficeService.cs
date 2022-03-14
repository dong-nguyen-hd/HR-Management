using Business.Domain.Models;
using Business.Resources.Office;

namespace Business.Domain.Services
{
    public interface IOfficeService : IBaseService<OfficeResource, CreateOfficeResource, UpdateOfficeResource, Office>
    {
    }
}
