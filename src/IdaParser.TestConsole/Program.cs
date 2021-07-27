﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IdaParser.TestConsole
{
    internal static class Program
    {
        private static async Task Main()
        {
            const string path = @"C:\Maplestory95.exe.h";
            var parser = new Parser();

            //var results = parser.SearchForStructInHeaderFile(path, "struct CItemInfo::INCLEVELITEM");
            //foreach(var rslt in results)
            //Console.WriteLine("Idx: {0} Word: {1}", rslt.Index, rslt.Word);

            List<Tuple<int, string>> results = new();
            string[] toSearchFor = { "struct CItemInfo::LevelInfo" };

            foreach (var str in toSearchFor)
                results.AddRange(parser.SearchForStructInHeaderFile2(path, str).ToList());

            if (!results.Any()) return;

            List<string> linesToWrite = new();
            const string openComment = "/*";
            var metaInfo = "* Autogenerated by IdaParser on: " + DateTime.Now;
            const string closeComment = "*/";

            linesToWrite.Add(openComment);
            linesToWrite.Add(metaInfo);
            linesToWrite.Add(closeComment);
            linesToWrite.AddRange(results.Select(rslt => rslt.Item2));

            foreach (var line in linesToWrite)
                Console.WriteLine("Line: {0}", line);

            await File.WriteAllLinesAsync(@"C:\test.txt", linesToWrite);
        }
    }
}
