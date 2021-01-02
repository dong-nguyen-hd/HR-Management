using HR_Management.Domain.Models;

namespace HR_Management.Domain.Services.Communication
{
    public class TechnologyResponse : BaseResponse<Technology>
    {
        public TechnologyResponse(bool isSuccess) : base(isSuccess) { }

        public TechnologyResponse(Technology technology) : base(technology) { }

        public TechnologyResponse(object objTechnology) : base(objTechnology) { }

        public TechnologyResponse(string message) : base(message) { }
    }
}
