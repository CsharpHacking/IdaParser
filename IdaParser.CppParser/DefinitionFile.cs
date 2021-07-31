using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils.Enumeration;

namespace IdaParser.CppParser
{
    public class DefinitionFile
    {
        public List<Tuple<int, string>> pragmas;
        public List<Tuple<int, string>> includes;
        public List<Tuple<int, string>> classes;
        public List<Tuple<int, string>> structures;

        public DefinitionFile()
        {
            pragmas =  new List<Tuple<int, string>>();
            includes = new List<Tuple<int, string>>();
            classes = new List<Tuple<int, string>>();
            structures = new List<Tuple<int, string>>();
        }

        public void Parse(string pathToHeaderFile)
        {
            var lines = File.ReadLines(pathToHeaderFile).ToList();
            Console.WriteLine("Lines: {0}", lines.Count);

            foreach (var (line, idx) in lines.WithIndex())
            {
                if (line.Equals(string.Empty) || string.IsNullOrWhiteSpace(line)) continue;
                if (line.StartsWith("/") || line.StartsWith("0") || line.StartsWith("*") || line.StartsWith("*")) continue;

                var curLine = line.Trim();

                if (curLine.StartsWith("#pragma"))
                {
                    pragmas.Add(new Tuple<int, string>(idx, curLine));
                }
                else if (curLine.StartsWith("#include"))
                {
                    includes.Add(new Tuple<int, string>(idx, curLine));
                }
                else if (curLine.StartsWith("class"))
                {
                    classes.Add(new Tuple<int, string>(idx, curLine));
                }
                else if (curLine.StartsWith("struct"))
                {
                    structures.Add(new Tuple<int, string>(idx, curLine));
                }
            }
        }
    }
}
