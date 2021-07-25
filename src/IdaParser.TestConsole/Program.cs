using System;

namespace IdaParser.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            const string path = @"C:\Maplestory95.exe.h";

            var parser = new Parser();
            var results = parser.SearchForStructureInHeaderFile(path, "CItemInfo::INCLEVELITEM");

            foreach(var rslt in results)
            {
                Console.WriteLine(rslt);
            }
        }
    }
}
