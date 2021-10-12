using Business.Domain.Models;
using Business.Resources.Location;

namespace Business.Domain.Services
{
    public interface ILocationService : IBaseService<LocationResource, CreateLocationResource, UpdateLocationResource, Location>
    {
    }
}
