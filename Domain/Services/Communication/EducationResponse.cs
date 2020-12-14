using HR_Management.Domain.Models;

namespace HR_Management.Domain.Services.Communication
{
    public class EducationResponse: BaseResponse<Education>
    {
        public EducationResponse(Education education) : base(education) { }

        public EducationResponse(string message) : base(message) { }
    }
}
