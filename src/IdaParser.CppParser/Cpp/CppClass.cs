using System.Collections.Generic;

namespace IdaParser.CppParser.Cpp
{
    public class CppClass
    {
        public List<CppClass> memberClass;
        public List<CppStruct> memberStruct;
        public List<CppVar> memberVar;

        public CppClass()
        {
            memberClass = new List<CppClass>();
            memberStruct = new List<CppStruct>();
            memberVar = new List<CppVar>();
        }
    }
}