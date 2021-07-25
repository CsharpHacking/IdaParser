using Ganss.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils.Types.String;

namespace IdaParser
{
    public class Parser
    {
        public void ConvertStructToCpp(string structText)
        {
        }

        public IEnumerable<string> SearchForStructureInHeaderFile(string pathToHeaderFile, string structureName)
        {
            if (!pathToHeaderFile.Valid())
                return null;

            List<string> lines = new();
            foreach (var searchLine in File.ReadLines(pathToHeaderFile))
                lines.Add(searchLine);

            Console.WriteLine(lines.Count);

            var searchText = new AhoCorasick(lines);
            return (IEnumerable<string>) searchText.Search(structureName).ToList();
        }
    }
}
