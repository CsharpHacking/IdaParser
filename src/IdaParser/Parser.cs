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
                    results.Add(new Tuple<int, string>(idx + 1, line));

            return results;
        }
    }
}
