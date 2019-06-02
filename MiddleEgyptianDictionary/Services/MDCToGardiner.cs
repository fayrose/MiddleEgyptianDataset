using MiddleEgyptianDictionary.Properties;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MiddleEgyptianDictionary.Services
{
    static class MDCToGardiner
    {
        static Dictionary<String, String> phoneticToGardiner = BuildPhoneticsToGardiner();
        static Dictionary<String, String> numbersToGardiner = BuildNumbersToGardiner();
        static Dictionary<String, String> jseshGardinerNormalizer = BuildJseshGardinerNormalizer();
        
        private static Dictionary<String, String> BuildPhoneticsToGardiner()
        {
            String[] arr = Resources.MdcToGardiner.Split('\n');
            Dictionary<String, String> dic = new Dictionary<string, string>();
            foreach (String line in arr) {
                String[] splitLine = line.Split(',');
                dic.Add(splitLine[1], splitLine[0]);
            }
            return dic;
        }

        private static Dictionary<String, String> BuildNumbersToGardiner()
        {
            Dictionary<String, String> dic = new Dictionary<string, string>();
            dic.Add("1", "Z15");
            dic.Add("2", "Z15A");
            dic.Add("3", "Z15B");
            dic.Add("4", "Z15C");
            dic.Add("5", "Z15I");
            dic.Add("10", "V20");
            dic.Add("20", "V20I");
            dic.Add("30", "V20J");
            dic.Add("40", "V20K");
            dic.Add("50", "V20L");
            dic.Add("100", "V1");
            dic.Add("200", "V1A");
            dic.Add("300", "V1B");
            dic.Add("400", "V1C");
            dic.Add("500", "V1I");
            return dic;
        }

        private static Dictionary<String, String> BuildJseshGardinerNormalizer()
    {
            // Differences between JSesh and Unicode Gardiners are described here:
            // https://github.com/rosmord/jsesh/blob/8263fdd4f8fdb9dbd29235ab015e6a7625a9ebd6/jsesh/src/main/resources/jsesh/graphics/glyphs/svgFontExporter/MdC2Unicode-table.txt#L187

            Dictionary<String, String> dic = new Dictionary<string, string>();
            dic.Add("A133", "A6A");
            dic.Add("A32F", "A32A");
            dic.Add("A43D", "A43A");
            dic.Add("A45B", "A45A");
            dic.Add("A58B", "A58");
            dic.Add("A116", "A64");
            dic.Add("US1A6BEXTU", "A6B");
            dic.Add("A469", "A65");
            dic.Add("A239", "A66");
            dic.Add("A282", "A67");
            dic.Add("A199A", "A068");
            dic.Add("A73", "A70");
            dic.Add("B104", "B5A");
            dic.Add("B47", "B9");
            dic.Add("US1C2BEXTU", "C2B");
            dic.Add("US1C2CEXTU", "C2C");
            dic.Add("US1C13EXTU", "C13");
            dic.Add("US1C15EXTU", "C15");
            dic.Add("C33", "C21");
            dic.Add("C49B", "C22");
            dic.Add("US1D50AEXTU", "D50A");
            dic.Add("US1D50BEXTU", "D50B");
            dic.Add("US1D50CEXTU", "D50C");
            dic.Add("US1D50DEXTU", "D50D");
            dic.Add("US1D50EEXTU", "D50E");
            dic.Add("US1D50FEXTU", "D50F");
            dic.Add("US1D50GEXTU", "D50G");
            dic.Add("US1D50HEXTU", "D50H");
            dic.Add("US1D50IEXTU", "D50I");
            dic.Add("S125", "D52A");
            dic.Add("D271", "D64");
            dic.Add("D132", "D65");
            dic.Add("N33", "D67");
            dic.Add("US1D67AEXTU", "D67A");
            dic.Add("US1D67BEXTU", "D67B");
            dic.Add("US1D67CEXTU", "D67C");
            dic.Add("US1D67DEXTU", "D67D");
            dic.Add("US1D67EEXTU", "D67E");
            dic.Add("US1D67FEXTU", "D67F");
            dic.Add("US1D67GEXTU", "D67G");
            dic.Add("US1D67HEXTU", "D67H");
            dic.Add("E100", "E9A");
            dic.Add("E16AEXTU", "E16A");
            dic.Add("E141", "E17A");
            dic.Add("E147", "E20A");
            dic.Add("US1E28AEXTU", "E28A");
            dic.Add("E34", "E34A");
            dic.Add("E35", "E36");
            dic.Add("E45", "E37");
            dic.Add("E92", "E38");
            dic.Add("F63", "F1A");
            dic.Add("F13", "F13A");
            dic.Add("FF4", "F21A");
            dic.Add("US22F31AEXTU", "F31A");
            dic.Add("F37B", "F37A");
            dic.Add("F38C", "F38A");
            dic.Add("AA56", "F45A");
            dic.Add("US1F51AEXTU", "F51A");
            dic.Add("US1F51BEXTU", "F51B");
            dic.Add("F51A", "F51C");
            dic.Add("F59", "F53");
            dic.Add("G139", "G6A");
            dic.Add("G90", "G20A");
            dic.Add("G36", "G36A");
            dic.Add("G37", "G37A");
            dic.Add("US1G43AEXTU", "G43A");
            dic.Add("G247", "G45A");
            dic.Add("I24", "I9A");
            dic.Add("I31", "I10A");
            dic.Add("US1I11AEXTU", "I11A");
            dic.Add("K13", "K8");
            dic.Add("US1L2AEXTU", "L2A");
            dic.Add("US1L3EXTU", "L3");
            dic.Add("US1L6AEXTU", "L6A");
            dic.Add("M1E", "M1B");
            dic.Add("M72", "M10A");
            dic.Add("US1M12AEXTU", "M12A");
            dic.Add("US1M12BEXTU", "M12B");
            dic.Add("US1M12CEXTU", "M12C");
            dic.Add("US1M12DEXTU", "M12D");
            dic.Add("US1M12EEXTU", "M12E");
            dic.Add("US1M12FEXTU", "M12F");
            dic.Add("US1M12GEXTU", "M12G");
            dic.Add("US1M12HEXTU", "M12H");
            dic.Add("M139", "M15A");
            dic.Add("M139B", "M16A");
            dic.Add("US1M17AEXTU", "M17A");
            dic.Add("NN", "M22A");
            dic.Add("M127", "M24A");
            dic.Add("M140B", "M28A");
            dic.Add("US1M31AEXTU", "M31A");
            dic.Add("US1M33BEXTU", "M33B");
            dic.Add("US1M40AEXTU", "M40A");
            dic.Add("N102", "N18A");
            dic.Add("O239", "N18B");
            dic.Add("XAST", "N25A");
            dic.Add("N34", "N34A");
            dic.Add("O54", "O1A");
            dic.Add("O5U", "O5A");
            dic.Add("US1O6AEXTU", "O6A");
            dic.Add("US1O6BEXTU", "O6B");
            dic.Add("US1O6CEXTU", "O6C");
            dic.Add("US1O6DEXTU", "O6D");
            dic.Add("US1O6EEXTU", "O6E");
            dic.Add("US1O6FEXTU", "O6F");
            dic.Add("US1O10BEXTU", "O10B");
            dic.Add("US248O10CEXTU", "O10C");
            dic.Add("O190", "O25A");
            dic.Add("O29V", "O29A");
            dic.Add("US1O30AEXTU", "O30A");
            dic.Add("US1O33AEXTU", "O33A");
            dic.Add("US1O36AEXTU", "O36A");
            dic.Add("US1O36BEXTU", "O36B");
            dic.Add("US1O36CEXTU", "O36C");
            dic.Add("US1O36DEXTU", "O36D");
            dic.Add("FF8", "O50A");
            dic.Add("FF8A", "O50B");
            dic.Add("US1P3AEXTU", "P3A");
            dic.Add("R1H", "R2A");
            dic.Add("R3P", "R3A");
            dic.Add("R3PA", "R3B");
            dic.Add("R50", "R10A");
            dic.Add("R16B", "R16A");
            dic.Add("T63B", "R27");
            dic.Add("R129", "R28");
            dic.Add("O196", "R29");
            dic.Add("S48", "S2A");
            dic.Add("S50", "S6A");
            dic.Add("US1S14BEXTU", "S14B");
            dic.Add("S197", "S17A");
            dic.Add("IW", "S26A");
            dic.Add("US1S26BEXTU", "S26B");
            dic.Add("US1S35ABEXTU", "S35A");
            dic.Add("S56", "S46");
            dic.Add("T43", "T3A");
            dic.Add("T60", "T11A");
            dic.Add("S123", "T32A");
            dic.Add("US1T33AEXTU", "T33A");
            dic.Add("US1U6AEXTU", "U6A");
            dic.Add("US1U6BEXTU", "U6B");
            dic.Add("FF7", "U32A");
            dic.Add("O30U", "U42");
            dic.Add("200", "V1A");
            dic.Add("300", "V1B");
            dic.Add("400", "V1C");
            dic.Add("US1V1DEXTU", "V1D");
            dic.Add("US1V1EEXTU", "V1E");
            dic.Add("US1V1FEXTU", "V1F");
            dic.Add("US1V1GEXTU", "V1G");
            dic.Add("US1V1HEXTU", "V1H");
            dic.Add("FF6", "V2A");
            dic.Add("V60", "V7A");
            dic.Add("V49A", "V7B");
            dic.Add("US1V11AEXTU", "V11A");
            dic.Add("US1V11BEXTU", "V11B");
            dic.Add("US1V11CEXTU", "V11C");
            dic.Add("H27", "V12A");
            dic.Add("US1V12BEXTU", "V12B");
            dic.Add("US1V20AEXTU", "V20A");
            dic.Add("US1V20BEXTU", "V20B");
            dic.Add("US1V20CEXTU", "V20C");
            dic.Add("US1V20DEXTU", "V20D");
            dic.Add("US1V20EEXTU", "V20E");
            dic.Add("US1V20FEXTU", "V20F");
            dic.Add("US1V20GEXTU", "V20G");
            dic.Add("US1V20HEXTU", "V20H");
            dic.Add("V71", "V28A");
            dic.Add("V81", "V29A");
            dic.Add("NB", "V30A");
            dic.Add("US1V33AEXTU", "V33A");
            dic.Add("F143A", "V37A");
            dic.Add("V20H", "V40");
            dic.Add("US1V40AEXTU", "V40A");
            dic.Add("W3", "W3A");
            dic.Add("W9R", "W9A");
            dic.Add("US1W14AEXTU", "W14A");
            dic.Add("US22W17BVARE", "W17A");
            dic.Add("US22W17BVARD", "W18A");
            dic.Add("US1W24AEXTU", "W24A");
            dic.Add("N18", "X4B");
            dic.Add("US1X6AEXTU", "X6A");
            dic.Add("M44", "X8A");
            dic.Add("Y1V", "Y1A");
            dic.Add("US1Z2AEXTU", "Z2A");
            dic.Add("N33A", "Z2B");
            dic.Add("Z2A", "Z2C");
            dic.Add("Z2B", "Z2D");
            dic.Add("N33AV", "Z3B");
            dic.Add("FF203", "Z5A");
            dic.Add("US1Z13EXTU", "Z13");
            dic.Add("FF1", "Z14");
            dic.Add("FF301", "Z15");
            dic.Add("FF302", "Z15A");
            dic.Add("FF303", "Z15B");
            dic.Add("FF304", "Z15C");
            dic.Add("US1Z15DEXTU", "Z15D");
            dic.Add("US1Z15EEXTU", "Z15E");
            dic.Add("US1Z15FEXTU", "Z15F");
            dic.Add("US1Z15GEXTU", "Z15G");
            dic.Add("US1Z15HEXTU", "Z15H");
            dic.Add("Z1A", "Z16");
            dic.Add("Z4B", "Z16A");
            dic.Add("US1Z16BEXTU", "Z16B");
            dic.Add("US1Z16CEXTU", "Z16C");
            dic.Add("US1Z16DEXTU", "Z16D");
            dic.Add("US1Z16EEXTU", "Z16E");
            dic.Add("US1Z16FEXTU", "Z16F");
            dic.Add("US1Z16GEXTU", "Z16G");
            dic.Add("US1Z16HEXTU", "Z16H");
            dic.Add("US1AA7AEXTU", "AA7A");

            return dic;
        }

        public static string GetGardinerFromPhonetics(string mdc)
        {
            if (phoneticToGardiner.TryGetValue(mdc, out string gardiner))
            {
                return gardiner;
            }
            return null;
        }

        public static string GetGardinerFromNumbers(string mdc)
        {
            if (numbersToGardiner.TryGetValue(mdc, out string gardiner))
            {
                return gardiner;
            }
            return null;
        }

        public static string NormalizeGardinerCode(string mdc)
        {
            // Note - Only codes obtained through JSesh must be normalized.
            // As gardiner codes from the dictionaries are normalized, this
            // should only be for trigrams taken from the Jsesh files. 
            if (jseshGardinerNormalizer.TryGetValue(mdc, out string val))
            {
                return val;
            }
            return mdc;
        }

        public static string NormalizeGardinerString(string mdc)
        {
            int idx = 0;
            string answer = "";
            string temp = "";
            while (idx < mdc.Length)
            {
                if (mdc[idx].Equals('-') ||
                    mdc[idx].Equals('&') || 
                    mdc[idx].Equals('*') ||
                    mdc[idx].Equals(':'))
                {
                    answer += NormalizeGardinerCode(temp) + mdc[idx];
                    temp = "";
                }
                else
                {
                    temp += mdc[idx];
                }
                idx++;
            }
            answer += NormalizeGardinerCode(temp);
            return answer;
        }

        public static string GetPhonetics(string gardiner)
        {
            if (phoneticToGardiner.ContainsKey(gardiner))
            {
                return phoneticToGardiner[gardiner];
            }
            return null;
        }
    }
}
