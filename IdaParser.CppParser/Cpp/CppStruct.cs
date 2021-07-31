using System.Collections.Generic;

namespace IdaParser.CppParser.Cpp
{
    public class CppStruct
    {
        public List<CppVar> memberVar;

        public CppStruct()
        {
            memberVar = new List<CppVar>();
        }
    }
}