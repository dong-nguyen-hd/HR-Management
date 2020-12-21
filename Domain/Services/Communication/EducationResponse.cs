using HR_Management.Domain.Models;

namespace HR_Management.Domain.Services.Communication
{
    public class EducationResponse: BaseResponse<Education>
    {
        public EducationResponse(Education education) : base(education) { }

        public EducationResponse(object objEducation) : base(objEducation) { }

        public EducationResponse(string message) : base(message) { }
    }
}
