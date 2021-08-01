namespace IdaParser.CppParser.Cpp
{
    public class CppVar
    {
        public string DataType;
        public string Name;

        public CppVar(string dataType, string name)
        {
            DataType = dataType;
            Name = name;
        }
    }
}