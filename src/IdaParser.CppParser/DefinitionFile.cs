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
        public List<CppClass> classes;
        public List<CppStruct> structures;

        public DefinitionFile()
        {
            pragmas =  new List<Tuple<int, string>>();
            includes = new List<Tuple<int, string>>();
            classes = new List<CppClass>();
            structures = new List<CppStruct>();
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

                    cppClass = new CppClass(idx, line);

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

                        classes.Add(cppClass);
                        i++;
                    }
                }
                else if (curLine.Contains("struct"))
                {
                    structures.Add(new CppStruct(idx, curLine));
                }
                else if (!curLine.Contains("{") && !curLine.Contains("};") && !curLine.Contains("//"))
                {
                    var basicType = GetBasicType(curLine);
                    if (!string.IsNullOrEmpty(basicType))
                    {
                        var varName = curLine.Replace(basicType, string.Empty).Trim();

                        cppClass.memberVar.Add(new CppVar(basicType, varName));
                        continue;
                    }

                    var curLineSplit = curLine.SplitIfNotPrecededByChar(" ", ',');
                    var dataType = curLineSplit[0];
                    var name = curLineSplit[1].Trim();

                    cppClass.memberVar.Add(new CppVar(dataType, name));
                }
            }
        }

        public string GetBasicType(string line)
        {
            var sizedIntegers = (from SizedIntegers enumEntry 
                in Enum.GetValues(typeof(SizedIntegers)) 
                select enumEntry.GetDescription()).ToList();
            var cppDataTypes = (from CppDataTypes enumEntry 
                in Enum.GetValues(typeof(CppDataTypes))
                select enumEntry.GetDescription()).ToList();

            var orderedSizedIntegers = sizedIntegers.OrderByDescending(x => x.Length).ToList();
            foreach (var sizedInteger in orderedSizedIntegers.Where(sizedInteger 
                => sizedInteger.StrictlyCharEqualTo(line.HeadSizeOfStrB(sizedInteger))))
                return sizedInteger;

            var orderedCppDataTypes = cppDataTypes.OrderByDescending(x => x.Length).ToList();
            foreach (var cppDataType in orderedCppDataTypes.Where(cppDataType
                => cppDataType.StrictlyCharEqualTo(line.HeadSizeOfStrB(cppDataType))))
                return cppDataType;

            return string.Empty;
        }
    }
}
