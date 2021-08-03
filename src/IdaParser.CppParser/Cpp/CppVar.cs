namespace IdaParser.CppParser.Cpp
{
    public class CppVar : CppObj
    {
        public string DataType;

        public CppVar(string dataType, string name)
        {
            DataType = dataType;
            Name = name;
        }
    }
}