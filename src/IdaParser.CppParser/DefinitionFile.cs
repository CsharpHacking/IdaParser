using IdaParser.CppParser.Cpp;
using IdaParser.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils.Enumeration;
using Utils.Types.Enum;
using Utils.Types.String;

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

                if (curLine.Contains("/") || curLine.Contains("0") ||
                    curLine.Contains("*") || curLine.Contains("/*") || curLine.Contains("*/")) continue;

                if (curLine.Contains("#pragma"))
                {
                    pragmas.Add(new Tuple<int, string>(idx, curLine));
                }
                else if (curLine.Contains("#include"))
                {
                    includes.Add(new Tuple<int, string>(idx, curLine));
                }
                else if (curLine.Contains("class"))
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

                        if (nextLine.Contains("class"))
                        {
                            cppClass.memberClass.Add(new CppClass());
                        }
                        else if (nextLine.Contains("struct"))
                        {
                            cppClass.memberStruct.Add(new CppStruct());
                        }

                        classes.Add(new Tuple<int, CppClass>(idx, cppClass));
                        i++;
                    }
                }
                else if (curLine.Contains("struct"))
                {
                    structures.Add(new Tuple<int, string>(idx, curLine));
                }
                else if (!curLine.Contains("{") && !curLine.Contains("};") && !curLine.Contains("//"))
                {
                    //check basic types
                    var basicType = GetBasicType(curLine);
                    if (!string.IsNullOrEmpty(basicType))
                    {
                        cppClass.memberVar.Add(new CppVar(basicType,
                                curLine.Replace(basicType, string.Empty)));

                        continue;
                    }

                    //TODO: split at index
                    var curLineSplit = curLine.Split(" ", 2);
                    var dataType = curLineSplit[0];
                    var name = curLineSplit[1];

                    cppClass.memberVar.Add(new CppVar(dataType, name));
                }
            }
        }

        public string GetBasicType(string line)
        {
            foreach (SizedIntegers enumEntry in Enum.GetValues(typeof(SizedIntegers)))
            {
                var enumDescription = enumEntry.GetDescription();

                if (enumDescription.StrictlyCharEqualTo(line.HeadSizeOfStrB(enumDescription)))
                    return enumDescription;
            }
            foreach (CppDataTypes enumEntry in Enum.GetValues(typeof(CppDataTypes)))
            {
                var enumDescription = enumEntry.GetDescription();

                if (enumDescription.StrictlyCharEqualTo(line.HeadSizeOfStrB(enumDescription)))
                    return enumDescription;
            }

            return string.Empty;
        }
    }
}
