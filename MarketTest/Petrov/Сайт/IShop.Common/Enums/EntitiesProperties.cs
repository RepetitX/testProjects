using System.ComponentModel;

namespace IShop.Common.Enums
{
    public enum Gender : byte
    {
        [Description("Господин")]
        Male = 1,

        [Description("Госпожа")]
        Female = 2
    }
}