using System;
using System.Collections.Generic;
using System.Linq;

namespace IdaParser.TestConsole
{
    internal static class Program
    {
        private static void Main()
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

            List<string> linesToWrite = new();
            const string openComment = "/*";
            const string closeComment = "*/";

            linesToWrite.Add(openComment);
            linesToWrite.Add(closeComment);
            linesToWrite.AddRange(results.Select(rslt => rslt.Item2));

            foreach (var line in linesToWrite)
                Console.WriteLine("Line: {0}", line);
        }
    }
}
