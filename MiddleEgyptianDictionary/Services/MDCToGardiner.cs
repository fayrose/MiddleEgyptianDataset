using MiddleEgyptianDictionary.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MiddleEgyptianDictionary.Services
{
    static class MDCToGardiner
    {
        static Dictionary<String, String> phoneticToGardiner = BuildPhoneticsToGardiner();
        static Dictionary<String, String> numbersToGardiner = BuildNumbersToGardiner();
        
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

        public static string GetPhonetics(string gardiner)
        {
            if (phoneticToGardiner.ContainsKey(gardiner))
            {
                return phoneticToGardiner[gardiner];
            }
            return null;
        }


        public static void ConvertDirectory(String directory)
        {
            foreach (String file in Directory.GetFiles(directory))
            {
                ConvertFile(Path.Combine(directory, file));
            }
        }

        private static void ConvertFile(String filePath)
        {
            StringBuilder convertedFile = new StringBuilder();
            String[] fileText = File.ReadAllLines(filePath);
            foreach (String line in fileText)
            {
                String converted = ConvertLine(line);
                if (converted != null)
                {
                    convertedFile.AppendLine(converted);
                }
            }
        }

        private static string ConvertLine(string line)
        {
            throw new NotImplementedException();
        }
    }
}
