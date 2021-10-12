using System.ComponentModel;

namespace Business.Data
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
