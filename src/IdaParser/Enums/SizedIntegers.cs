using System.ComponentModel;

namespace IdaParser.Enums
{
    public enum SizedIntegers
    {
        [Description("__int8")]
        Char = 1,
        [Description("__int16")]
        Short = 2,
        [Description("__int32")]
        Int = 3,
        [Description("__int64")]
        LongLong = 4
    }
}