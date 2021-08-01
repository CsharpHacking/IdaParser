using IdaParser.CppParser.Cpp;
using IdaParser.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utils.Enumeration;
using Utils.Types.Enum;

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

        public bool AreStringsEqual(string strA, string strB)
        {
            var result = string.CompareOrdinal(strA, strB);

            return result == 0;
        }

        public bool AreCharEqual(string strA, string strB)
        {
            var aSet = new HashSet<char>(strA);
            var bSet = new HashSet<char>(strB);

            var areHashEqual = aSet.SetEquals(bSet);
            var areStringEqual = AreStringsEqual(strA, strB);

            Console.WriteLine("strA {0} strB: {1} HashEqual: {2} StringEqual: {3}", strA, strB, areHashEqual, areStringEqual);

            return areHashEqual && areStringEqual;
        }

        public string GetSubStringSizeOfTypeStr(string str, string typeStr)
        {
            return str.Length >= typeStr.Length ? str[..typeStr.Length] : string.Empty;
        }

        public string GetBasicType(string line)
        {
            return line switch
            {
                _ when line.Contains(SizedIntegers.Char.GetDescription()) 
                    => SizedIntegers.Char.GetDescription(),
                _ when line.Contains(SizedIntegers.Short.GetDescription())
                    => SizedIntegers.Short.GetDescription(),
                _ when line.Contains(SizedIntegers.Int.GetDescription())
                    => SizedIntegers.Int.GetDescription(),
                _ when line.Contains(SizedIntegers.LongLong.GetDescription())
                    => SizedIntegers.LongLong.GetDescription(),

                _ when AreCharEqual(CppDataTypes.UnsignedShortInt.GetDescription(),
                        GetSubStringSizeOfTypeStr(line, CppDataTypes.UnsignedShortInt.GetDescription()))
                    => CppDataTypes.UnsignedShortInt.GetDescription(),
                _ when AreCharEqual(CppDataTypes.UnsignedLongInt.GetDescription(),
                        GetSubStringSizeOfTypeStr(line, CppDataTypes.UnsignedLongInt.GetDescription()))
                    => CppDataTypes.UnsignedLongInt.GetDescription(),
                _ when AreCharEqual(CppDataTypes.SignedShortInt.GetDescription(),
                        GetSubStringSizeOfTypeStr(line, CppDataTypes.SignedShortInt.GetDescription()))
                    => CppDataTypes.SignedShortInt.GetDescription(),
                _ when AreCharEqual(CppDataTypes.SignedLongInt.GetDescription(),
                        GetSubStringSizeOfTypeStr(line, CppDataTypes.SignedLongInt.GetDescription()))
                    => CppDataTypes.SignedLongInt.GetDescription(),
                _ when AreCharEqual(CppDataTypes.UnsignedShort.GetDescription(),
                        GetSubStringSizeOfTypeStr(line, CppDataTypes.UnsignedShort.GetDescription()))
                    => CppDataTypes.UnsignedShort.GetDescription(),
                _ when AreCharEqual(CppDataTypes.UnsignedLong.GetDescription(),
                        GetSubStringSizeOfTypeStr(line, CppDataTypes.UnsignedLong.GetDescription()))
                    => CppDataTypes.UnsignedLong.GetDescription(),
                _ when AreCharEqual(CppDataTypes.UnsignedInt.GetDescription(),
                        GetSubStringSizeOfTypeStr(line, CppDataTypes.UnsignedInt.GetDescription()))
                    => CppDataTypes.UnsignedInt.GetDescription(),
                _ when AreCharEqual(CppDataTypes.SignedShort.GetDescription(),
                        GetSubStringSizeOfTypeStr(line, CppDataTypes.SignedShort.GetDescription()))
                    => CppDataTypes.SignedShort.GetDescription(),
                _ when AreCharEqual(CppDataTypes.SignedLong.GetDescription(),
                        GetSubStringSizeOfTypeStr(line, CppDataTypes.SignedLong.GetDescription()))
                    => CppDataTypes.SignedLong.GetDescription(),
                _ when AreCharEqual(CppDataTypes.SignedLong.GetDescription(),
                        GetSubStringSizeOfTypeStr(line, CppDataTypes.SignedLong.GetDescription()))
                    => CppDataTypes.SignedLong.GetDescription(),
                _ when AreCharEqual(CppDataTypes.SignedInt.GetDescription(),
                        GetSubStringSizeOfTypeStr(line, CppDataTypes.SignedInt.GetDescription()))
                    => CppDataTypes.SignedInt.GetDescription(),
                _ when AreCharEqual(CppDataTypes.ShortInt.GetDescription(),
                        GetSubStringSizeOfTypeStr(line, CppDataTypes.ShortInt.GetDescription()))
                    => CppDataTypes.ShortInt.GetDescription(),
                _ when AreCharEqual(CppDataTypes.Unsigned.GetDescription(),
                        GetSubStringSizeOfTypeStr(line, CppDataTypes.Unsigned.GetDescription()))
                    => CppDataTypes.Unsigned.GetDescription(),
                _ when AreCharEqual(CppDataTypes.LongInt.GetDescription(),
                        GetSubStringSizeOfTypeStr(line, CppDataTypes.LongInt.GetDescription()))
                    => CppDataTypes.LongInt.GetDescription(),
                _ when AreCharEqual(CppDataTypes.Signed.GetDescription(),
                        GetSubStringSizeOfTypeStr(line, CppDataTypes.Signed.GetDescription()))
                    => CppDataTypes.Signed.GetDescription(),
                _ when AreCharEqual(CppDataTypes.Short.GetDescription(),
                        GetSubStringSizeOfTypeStr(line, CppDataTypes.Short.GetDescription()))
                    => CppDataTypes.Short.GetDescription(),
                _ when AreCharEqual(CppDataTypes.Long.GetDescription(),
                        GetSubStringSizeOfTypeStr(line, CppDataTypes.Long.GetDescription()))
                    => CppDataTypes.Long.GetDescription(),
                _ when AreCharEqual(CppDataTypes.Int.GetDescription(),
                        GetSubStringSizeOfTypeStr(line, CppDataTypes.Int.GetDescription()))
                    => CppDataTypes.Int.GetDescription(),

                /*_ when line.Contains(CppDataTypes.LongLong.GetDescription())
                    => CppDataTypes.LongLong.GetDescription(),
                _ when line.Contains(CppDataTypes.LongLongInt.GetDescription())
                    => CppDataTypes.LongLongInt.GetDescription(),
                _ when line.Contains(CppDataTypes.SignedLongLong.GetDescription())
                    => CppDataTypes.SignedLongLong.GetDescription(),
                _ when line.Contains(CppDataTypes.SignedLongLongInt.GetDescription())
                    => CppDataTypes.SignedLongLongInt.GetDescription(),
                _ when line.Contains(CppDataTypes.UnsignedLongLong.GetDescription())
                    => CppDataTypes.UnsignedLongLong.GetDescription(),
                _ when line.Contains(CppDataTypes.UnsignedLongLongInt.GetDescription())
                    => CppDataTypes.UnsignedLongLongInt.GetDescription(), */

                _ => string.Empty
            };
        }
    }
}
