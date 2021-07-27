using Ganss.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils.Enumeration;
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

        public IEnumerable<Tuple<int, string>> SearchForStructInHeaderFile2(string pathToHeaderFile, string structName)
        {
            if (!pathToHeaderFile.Valid())
                return null;

            var results = new List<Tuple<int, string>>();
            var lines = File.ReadLines(pathToHeaderFile).ToList();

            Console.WriteLine("Lines: {0}", lines.Count);

            foreach (var (line, idx) in lines.WithIndex())
                if (line.Contains(structName))
                    if (line.Equals(structName))
                    {
                        if (line.Contains("::"))
                        {
                            var lineSplit = line.Split("::");
                            if (lineSplit.Length == 2)
                            {
                                var className = lineSplit[0].Replace("struct ", string.Empty);
                                var structureName = "struct " + lineSplit[1];

                                Console.WriteLine("className: {0}", className);

                                results.Add(new Tuple<int, string>(idx, structureName));
                            }
                        }

                        var nextLineIdx = idx + 1;
                        const string structOpenBrace = "{";
                        const string structClosingBrace = "};";
                        if (lines[nextLineIdx] != structOpenBrace)
                        {
                            Console.WriteLine("next line invalid: {0}", lines[nextLineIdx]);
                            continue;
                        }

                        results.Add(new Tuple<int, string>(nextLineIdx, lines[nextLineIdx]));

                        var i = 1;
                        while (lines[nextLineIdx + i] != structClosingBrace)
                        {
                            results.Add(new Tuple<int, string>(nextLineIdx + i, lines[nextLineIdx + i]));
                            i++;
                        }

                        if (lines[nextLineIdx + i] == structClosingBrace)
                        {
                            results.Add(new Tuple<int, string>(nextLineIdx + i, lines[nextLineIdx + i]));
                        }
                    }

            return results;
        }
    }
}
