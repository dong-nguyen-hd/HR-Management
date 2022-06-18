using System.ComponentModel;

namespace Business.Data
{
    public enum eRole
    {
        [Description("admin")]
        Admin = 1,

        [Description("editor")]
        Editor,

        [Description("viewer")]
        Viewer
    }
}
