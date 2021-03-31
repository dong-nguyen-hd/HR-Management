using System.ComponentModel;

namespace HR_Management.Infrastructure
{
    public enum eRole
    {
        [Description("admin")]
        Admin,

        [Description("editor")]
        Editor,

        [Description("viewer")]
        Viewer
    }
}
