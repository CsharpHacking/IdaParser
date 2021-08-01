using System.ComponentModel;

namespace IdaParser.Enums
{
    public enum CppDataTypes
    {
        [Description("short")]
        Short = 1,
        [Description("short int")]
        ShortInt = 2,
        [Description("signed short")]
        SignedShort = 3,
        [Description("signed short int")]
        SignedShortInt = 4,
        [Description("unsigned short")]
        UnsignedShort = 5,
        [Description("unsigned short int")]
        UnsignedShortInt = 6,
        [Description("int")]
        Int = 7,
        [Description("signed")]
        Signed = 8,
        [Description("signed int")]
        SignedInt = 9,
        [Description("unsigned")]
        Unsigned = 10,
        [Description("unsigned int")]
        UnsignedInt = 11,
        [Description("long")]
        Long = 12,
        [Description("long int")]
        LongInt = 13,
        [Description("signed long")]
        SignedLong = 14,
        [Description("signed long int")]
        SignedLongInt = 15,
        [Description("unsigned long")] 
        UnsignedLong = 16,
        [Description("unsigned long int")]
        UnsignedLongInt = 17,
        [Description("long long")]
        LongLong = 18,
        [Description("long long int")]
        LongLongInt = 19,
        [Description("signed long long")]
        SignedLongLong = 20,
        [Description("signed long long int")]
        SignedLongLongInt = 21,
        [Description("unsigned long long")]
        UnsignedLongLong = 22,
        [Description("unsigned long long int")]
        UnsignedLongLongInt = 23,
    }
}