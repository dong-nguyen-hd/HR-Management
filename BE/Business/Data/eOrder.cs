using System.ComponentModel;

namespace Business.Data
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
