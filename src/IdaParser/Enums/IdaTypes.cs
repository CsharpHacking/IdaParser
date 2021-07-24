using System.ComponentModel;

namespace IdaParser.Enums
{
    enum IdaDatatypes //we assume we diassemble on x86/x64 this will be false on other CPUs
    {
        [Description("db")]
        DataByte = 1,       //1 byte  | signed char/unsigned char
        [Description("dw")]
        DataWord = 2,       //2 bytes | short int/unsigned short int/wchar_t
        [Description("dd")]
        DataDoubleWord = 3, //4 bytes | unsigned int/int/long int/float/wchar_t
        [Description("dq")]
        DataQuadWord = 4    //8 bytes | unsigned long int/long long int/unsigned long long int/double
    }
}
