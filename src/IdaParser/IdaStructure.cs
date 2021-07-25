using IdaParser.Enums;
using Utils.Enumeration;
using Utils.Types.String;

namespace IdaParser
{
    /*
    00000000 CItemInfo::INCLEVELITEM struc ; (sizeof=0x8, align=0x4, copyof_6003)
    00000000 nItemID         dd ?
    00000004 nIncLEV         dd ?
    00000008 CItemInfo::INCLEVELITEM ends
    */
    public class IdaStructure
    {
        IdaStructureRowFirst FirstRow;
        IdaStructureRow[] MemberRows;
        IdaStructureRowLast LastRow;

        public IdaStructure(string inputStr)
        {
            var rows = inputStr.Split("\r\n");
            FirstRow = new IdaStructureRowFirst(rows[0]);

            var varRows = rows[1..(rows.Length - 1)];
            foreach((string row, int idx) in varRows.WithIndex())        
                MemberRows[idx] = new IdaStructureRow(row);

            LastRow = new IdaStructureRowLast(rows[rows.Length]);
        }
    }

    public class IdaStructureRowFirst
    {      
        int AddressSpace;
        string StructureName;
        string StructOpenTag = " struc ; ";
        ExtraInfo ExtraInfo;

        public IdaStructureRowFirst(string str)
        {
            var adrNamePlusExtraInfo = str.Split(StructOpenTag);
            var addressSpaceStructName = adrNamePlusExtraInfo[0].Split(" ");

            var addressSpace = addressSpaceStructName[0];
            if (addressSpace.IsDigitsOnly() && addressSpace.ToCharArray().Length == 8)
                AddressSpace = addressSpace.To<int>();
            
            var structName = addressSpaceStructName[1];
            if (structName.Contains("::"))
            {
                var classNameAndStructure = structName.Split("::");
                var className = classNameAndStructure[0];
                var structureName = classNameAndStructure[1];
                StructureName = structureName;
            }

            var extraInfoSplit = adrNamePlusExtraInfo[1].Split(", ", 2);
            var sizeOf = extraInfoSplit[0].Replace("sizeof=0x", string.Empty);
            var align = extraInfoSplit[1].Replace("align=0x", string.Empty);
            ExtraInfo = new ExtraInfo(sizeOf.To<int>(), align.To<int>());
        }
    }

    public class ExtraInfo
    {
        int SizeOfStructureBytes;
        int AlingmentOfStructureBytes;
        string CopyOfStructure;

        public ExtraInfo(int sizeOfStructureBytes, int alingmentOfStructureBytes)
        {
            SizeOfStructureBytes = sizeOfStructureBytes;
            AlingmentOfStructureBytes = alingmentOfStructureBytes;
        }
    }

    public class IdaStructureRow
    {
        int AddressSpace;
        string VarName;
        string VarIdaType;

        public IdaStructureRow(string str)
        {
            var rowSplit = str.Split(" ", 3);

            AddressSpace = rowSplit[0].To<int>();
            VarName = rowSplit[1];
            VarIdaType = rowSplit[2];
        }

        public string DecideVarType(IdaStructureRow varRow)
        {
            switch (varRow.VarIdaType.GetValueFromEnumDescription<IdaDatatypes>())
            {
                case IdaDatatypes.DataByte:
                    //check varPerfix for help
                    break;
                case IdaDatatypes.DataWord:
                    if (varRow.VarName.StartsWith("n") || varRow.VarName.StartsWith("t"))
                    {
                        return "int";
                    }
                    //check varPerfix for help
                    break;
                case IdaDatatypes.DataDoubleWord:
                    //check varPerfix for help
                    break;
                case IdaDatatypes.DataQuadWord:
                    //check varPerfix for help
                    break;

                default:
                    break;
            }

            return null;
        }
    }

    public class IdaStructureRowLast
    {      
        int AddressSpace;
        string StructureName;
        string StructEndTag = "ends";

        public IdaStructureRowLast(string str)
        {
            var rowSplit = str.Split(" ", 3);

            var addressSpace = rowSplit[0];
            if (addressSpace.IsDigitsOnly() && addressSpace.ToCharArray().Length == 8)
                AddressSpace = addressSpace.To<int>();

            var structName = rowSplit[1];
            if (structName.Contains("::"))
            {
                var classNameAndStructure = structName.Split("::");
                var className = classNameAndStructure[0];
                var structureName = classNameAndStructure[1];
                StructureName = structureName;
            }

            StructEndTag = rowSplit[2];
        }
    }
}
