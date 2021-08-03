using System.Collections.Generic;

namespace IdaParser.CppParser.Cpp
{
    public class CppClass : CppObj
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

        public CppClass(int lineIndex, string name)
        {
            LineIndex = lineIndex;
            Name = name;

            memberClass = new List<CppClass>();
            memberStruct = new List<CppStruct>();
            memberVar = new List<CppVar>();
        }
    }
}