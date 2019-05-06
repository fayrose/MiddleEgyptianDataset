using System;
using System.Collections.Generic;
using System.Linq;
using System.Unicode;

namespace MiddleEgyptianDictionary.Services
{
    static class UnicodeDictionaryBuilder
    {
        static Dictionary<string, string> unicodeDictionary;

        public static Dictionary<string, string> GetInstance()
        {
            if (unicodeDictionary == null)
            {
                unicodeDictionary = CreateUnicodeHashSet();
            }
            return unicodeDictionary;
        }

        private static Dictionary<string, string> CreateUnicodeHashSet()
        {
            Dictionary<string, string> unicodeTable = new Dictionary<string, string>();
            for (int i = 0x13000; i < 0x1342E; i++)
            {
                string unicodeString = char.ConvertFromUtf32(i);
                string unicodeName = UnicodeInfo.GetName(i).Split(' ').ToList().Last();
                unicodeTable.Add(unicodeName, unicodeString);
            }
            return unicodeTable;
        }
    }
}
