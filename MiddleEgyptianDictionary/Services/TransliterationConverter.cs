using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiddleEgyptianDictionary.Services
{
    class TransliterationConverter
    {
        static Dictionary<string, string> translitConverter = new Dictionary<string, string>();
        
        public TransliterationConverter()
        {
            translitConverter.Add("A", UnicodeNumberToString("A722"));
            translitConverter.Add("a", UnicodeNumberToString("A724"));
            translitConverter.Add("H", UnicodeNumberToString("1E25"));
            translitConverter.Add("x", UnicodeNumberToString("1E2B"));
            translitConverter.Add("X", UnicodeNumberToString("1E96"));
            translitConverter.Add("S", UnicodeNumberToString("0161"));
            translitConverter.Add("T", UnicodeNumberToString("1E6F"));
            translitConverter.Add("D", UnicodeNumberToString("1E0F"));
        }

        public static string PrettifyTransliteration(string input)
        {
            string answer = input;
            foreach (string letter in translitConverter.Keys)
            {
                answer = answer.Replace(letter, translitConverter[letter]);
            }
            return answer;
        }

        private static string UnicodeNumberToString(string input)
        {
            int code = int.Parse(input, System.Globalization.NumberStyles.HexNumber);
            return char.ConvertFromUtf32(code);
        }
    }
}
