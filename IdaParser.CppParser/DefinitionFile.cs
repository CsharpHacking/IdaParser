using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IdaParser.CppParser.Cpp;
using Utils.Enumeration;

namespace IdaParser.CppParser
{
    public class DefinitionFile
    {
        public List<Tuple<int, string>> pragmas;
        public List<Tuple<int, string>> includes;
        public List<Tuple<int, CppClass>> classes;
        public List<Tuple<int, string>> structures;

        public DefinitionFile()
        {
            pragmas =  new List<Tuple<int, string>>();
            includes = new List<Tuple<int, string>>();
            classes = new List<Tuple<int, CppClass>>();
            structures = new List<Tuple<int, string>>();
        }

        public void Parse(string pathToHeaderFile)
        {
            var lines = File.ReadLines(pathToHeaderFile).Where(line 
                => !line.Equals(string.Empty) || !string.IsNullOrWhiteSpace(line)).ToList();

            Console.WriteLine("Lines: {0}", lines.Count);

            var cppClass = new CppClass();

            foreach (var (line, idx) in lines.WithIndex())
            {
                var curLine = line.Trim();

                if (curLine.StartsWith("/") || curLine.StartsWith("0") ||
                    curLine.StartsWith("*") || curLine.StartsWith("/*") || curLine.StartsWith("*/")) continue;

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
                    var nextLineIdx = idx + 1;
                    const string structOpenBrace = "{";
                    const string structClosingBrace = "};";
                    if (lines[nextLineIdx] != structOpenBrace) 
                        throw new Exception("Bad class definition!");

                    var i = 1;
                    while (lines[nextLineIdx + i] != structClosingBrace)
                    {
                        var nextLine = lines[nextLineIdx + i];

                        if (nextLine.StartsWith("class"))
                        {
                            cppClass.memberClass.Add(new CppClass());
                        }
                        else if (nextLine.StartsWith("struct"))
                        {
                            cppClass.memberStruct.Add(new CppStruct());
                        }

                        classes.Add(new Tuple<int, CppClass>(idx, cppClass));
                        i++;
                    }
                }
                else if (curLine.StartsWith("struct"))
                {
                    structures.Add(new Tuple<int, string>(idx, curLine));
                }
                else if (!curLine.StartsWith("{") && !curLine.StartsWith("};") && !curLine.StartsWith("//"))
                {
                    //Todo: fix
                    var curLineSplit = curLine.Split(" ", 2);
                    var dataType = curLineSplit[0];
                    var name = curLineSplit[1];

                    cppClass.memberVar.Add(new CppVar(dataType, name));
                }
            }
        }
    }
}
