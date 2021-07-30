using Ganss.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IdaParser.Enums;
using Utils.Enumeration;
using Utils.Types.Enum;
using Utils.Types.String;

namespace IdaParser
{
    public class Parser
    {
        public void ConvertStructToCpp(string structText)
        {
        }

        public IEnumerable<WordMatch> SearchForStructInHeaderFile(string pathToHeaderFile, string structName)
        {
            if (!pathToHeaderFile.Valid())
                return null;

            var lines = 
                File.ReadLines(pathToHeaderFile).Where(searchLine 
                    => !string.IsNullOrEmpty(searchLine)).ToList();

            Console.WriteLine("Non-Empty Lines: {0}", lines.Count);

            var searchText = new AhoCorasick(lines);
            var result = searchText.Search(structName).ToList();

            if (result.Count == 0)
                Console.WriteLine("NOT FOUND!");

            return result;
        }

        private static Tuple<int, string> LocateStructureInText(IEnumerable<string> textLines, string structName)
        {
            foreach (var (line, idx) in textLines.WithIndex())
            {
                if (!line.Valid()) continue;
                if (!line.StartsWith("struct")) continue;
                if (line.EndsWith(";")) continue;

                var curLine = line;
                if (curLine.Split(" ", 2)[1].Contains("__cppobj "))
                    curLine = curLine.Replace("__cppobj ", string.Empty);

                if (curLine.Contains(structName))
                    return new Tuple<int, string>(idx, curLine);

                var lineWords = curLine.Split(" ");
                if (!lineWords.Where(word
                    => word.Contains(structName)).Any(word
                    => word.Equals(structName))) continue;

                return new Tuple<int, string>(idx, curLine);
            }

            return null;
        }

        public IEnumerable<Tuple<int, string>> SearchForStructInHeaderFile2(string pathToHeaderFile, string structName)
        {
            if (!pathToHeaderFile.Valid())
                return null;

            var classNameClean = string.Empty;
            var structureNameClean = string.Empty;
            var results = new List<Tuple<int, string>>();
            var lines = File.ReadLines(pathToHeaderFile).ToList();

            Console.WriteLine("Lines: {0}", lines.Count);
            var (idx, line) = LocateStructureInText(lines, structName);

            if (line.Contains(":"))
            {
                var curLineSplit = line.Split(":");
                var structureNameWhole = curLineSplit[0];

                if (curLineSplit[1].Contains(","))
                {
                    var baseClass = curLineSplit[1];
                }
                var baseClasses = curLineSplit[1];
            }

            if (line.Contains("::"))
            {
                var lineSplit = line.Split("::");
                switch (lineSplit.Length)
                {
                    case 2:
                    {
                        classNameClean = lineSplit[0].Replace("struct ", string.Empty);
                        structureNameClean = lineSplit[1];
                        var structureName = "struct " + structureNameClean;

                        results.Add(new Tuple<int, string>(idx, structureName));
                        break;
                    }
                    case 3:
                    {
                        classNameClean = lineSplit[0].Replace("struct ", string.Empty);
                        //TODO: structOrClass = lineSplit[1];
                        structureNameClean = lineSplit[2];
                        var structureName = "struct " + structureNameClean;

                        results.Add(new Tuple<int, string>(idx, structureName));
                        break;
                    }
                }
            }

            var nextLineIdx = idx + 1;
            const string structOpenBrace = "{";
            const string structClosingBrace = "};";
            if (lines[nextLineIdx] != structOpenBrace) return null;
            
            results.Add(new Tuple<int, string>(nextLineIdx, lines[nextLineIdx]));

            var i = 1;
            while (lines[nextLineIdx + i] != structClosingBrace)
            {
                var lineToWrite = lines[nextLineIdx + i];
                lineToWrite = lineToWrite switch
                {
                    _ when lineToWrite.Contains(SizedIntegers.Char.GetDescription()) 
                        => lineToWrite.Replace(SizedIntegers.Char.GetDescription(), "char"),
                    _ when lineToWrite.Contains(SizedIntegers.Short.GetDescription()) 
                        => lineToWrite.Replace(SizedIntegers.Short.GetDescription(), "short"),
                    _ when lineToWrite.Contains(SizedIntegers.Int.GetDescription()) 
                        => lineToWrite.Replace(SizedIntegers.Int.GetDescription(), "int"),
                    _ when lineToWrite.Contains(SizedIntegers.LongLong.GetDescription()) 
                        => lineToWrite.Replace(SizedIntegers.LongLong.GetDescription(), "long long"),
                    _ when lineToWrite.Contains("tagPOINT") 
                        => lineToWrite.Replace("tagPOINT", "POINT"),
                    _ when lineToWrite.Contains(classNameClean + "::" + structureNameClean + "::")
                        => lineToWrite.Replace(classNameClean + "::" + structureNameClean + "::", string.Empty),
                    _ when lineToWrite.Contains(classNameClean + "::")
                        => lineToWrite.Replace(classNameClean + "::", string.Empty),
                    _ when lineToWrite.TrimStart().StartsWith("int b")
                        => lineToWrite.Replace("int", "bool"),

                    _ => lineToWrite
                };

                var listOfIndices = lineToWrite.AllIndicesOf(" >").ToList();
                if (listOfIndices.Count is > 0 and < 2)
                {
                    var indexOfPattern = listOfIndices.First();
                    lineToWrite = lineToWrite.ReplaceAt(indexOfPattern, 1);
                }

                results.Add(new Tuple<int, string>(nextLineIdx + i, lineToWrite));
                i++;
            }

            if (lines[nextLineIdx + i] == structClosingBrace)
            {
                results.Add(new Tuple<int, string>(nextLineIdx + i, lines[nextLineIdx + i]));
            }

            return results;
        }
    }
}
