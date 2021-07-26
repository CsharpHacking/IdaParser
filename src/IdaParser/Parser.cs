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

        public IEnumerable<WordMatch> SearchForStructureInHeaderFile(string pathToHeaderFile, string structureName)
        {
            if (!pathToHeaderFile.Valid())
                return null;

            var lines = 
                File.ReadLines(pathToHeaderFile).Where(searchLine 
                    => !string.IsNullOrEmpty(searchLine)).ToList();

            Console.WriteLine(lines.Count);

            var searchText = new AhoCorasick(lines);
            var result = searchText.Search(structureName).ToList();

            if (result.Count == 0)
                Console.WriteLine("NOT FOUND!");

            return result;
        }
    }
}
