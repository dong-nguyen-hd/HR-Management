namespace HR_Management.Domain.Services.Communication
{
    public class ProjectResponse<T> : BaseResponse<T>
    {
        public ProjectResponse(T project) : base(project) { }

        public ProjectResponse(string message) : base(message) { }
    }
}
