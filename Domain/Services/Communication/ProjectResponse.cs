using HR_Management.Domain.Models;

namespace HR_Management.Domain.Services.Communication
{
    public class ProjectResponse : BaseResponse<Project>
    {
        public ProjectResponse(bool isSuccess) : base(isSuccess) { }

        public ProjectResponse(Project project) : base(project) { }

        public ProjectResponse(object objProject) : base(objProject) { }

        public ProjectResponse(string message) : base(message) { }
    }
}
