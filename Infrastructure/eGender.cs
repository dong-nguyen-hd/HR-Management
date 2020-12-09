using System.ComponentModel;

namespace HR_Management.Infrastructure
{
    public enum eGender : byte
    {
        [Description("Nam")]
        Male,

        [Description("Nữ")]
        Female,

        [Description("Giới tính khác")]
        Sexless
    }
}
