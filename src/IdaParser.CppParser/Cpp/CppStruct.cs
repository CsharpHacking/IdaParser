using System.Collections.Generic;

namespace IdaParser.CppParser.Cpp
{
    public class CppStruct : CppObj
    {
        public List<CppVar> memberVar;

        public CppStruct()
        {
            memberVar = new List<CppVar>();
        }

        public CppStruct(int lineIndex, string name)
        {
            LineIndex = lineIndex;
            Name = name;

            memberVar = new List<CppVar>();
        }
    }
}