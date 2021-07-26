using System;

namespace IdaParser.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            const string path = @"C:\Maplestory95.exe.h";

            var parser = new Parser();
            var results = parser.SearchForStructureInHeaderFile(path, "struct CItemInfo::INCLEVELITEM");

            foreach(var rslt in results)
            {
                Console.WriteLine("Idx: {0} Word: {1}", rslt.Index, rslt.Word);
            }
        }
    }
}
