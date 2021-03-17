using System.ComponentModel;

namespace HR_Management.Infrastructure
{
    public enum eOrder
    {
        [Description("Component: Work History")]
        WorkHistory,

        [Description("Component: Skill")]
        Skill,

        [Description("Component: Education")]
        Education,

        [Description("Component: Certificate")]
        Certificate,

        [Description("Component: Project")]
        Project,
    }
}
