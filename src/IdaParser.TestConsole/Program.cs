using System;
using System.Collections.Generic;
using System.Linq;

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

            List<Tuple<int, string>> results = new();
            string[] toSearchFor = { "struct CItemInfo::EQUIPITEM" };

            foreach (var str in toSearchFor)
                results.AddRange(parser.SearchForStructInHeaderFile2(path, str).ToList());

            if (!results.Any()) return;

            foreach (var rslt in results)
                Console.WriteLine("Idx: {0} Word: {1}", rslt.Item1, rslt.Item2);
        }
    }
}
