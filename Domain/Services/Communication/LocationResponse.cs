using HR_Management.Domain.Models;

namespace HR_Management.Domain.Services.Communication
{
    public class LocationResponse : BaseResponse<Location>
    {
        public LocationResponse(bool isSuccess) : base(isSuccess) { }

        public LocationResponse(Location location) : base(location) { }

        public LocationResponse(object objLocation) : base(objLocation) { }

        public LocationResponse(string message) : base(message) { }
    }
}
