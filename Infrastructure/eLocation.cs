using System.ComponentModel;

namespace HR_Management.Infrastructure
{
    public enum eLocation : byte
    {
        [Description("Hà Nội")]
        HAN,

        [Description("Đà Nẵng")]
        DN,

        [Description("Hồ Chí Minh")]
        HCM
    }
}
