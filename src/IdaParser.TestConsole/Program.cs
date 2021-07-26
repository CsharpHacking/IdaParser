using System;

namespace IdaParser.TestConsole
{
    class Program
    {
        static void Main()
        {
            const string path = @"C:\Maplestory95.exe.h";
            var parser = new Parser();

            //var results = parser.SearchForStructInHeaderFile(path, "struct CItemInfo::INCLEVELITEM");
            //foreach(var rslt in results)
                //Console.WriteLine("Idx: {0} Word: {1}", rslt.Index, rslt.Word);

            var results2 = parser.SearchForStructInHeaderFile2(path, "struct CItemInfo::INCLEVELITEM");
            foreach (var rslt in results2)
                Console.WriteLine("Idx: {0} Word: {1}", rslt.Item1, rslt.Item2);
        }
    }
}
