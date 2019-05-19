using System;
using System.Collections.Generic;
using System.Linq;
using System.Unicode;
using System.Text;

/***
 * This is a depreciated file for converting Gardiner signs into the Unicode 12.0 spec.
 * At the given time, there appears to be no fonts that actually implement Unicode 12.0's format
 * control's for heiroglyphics. I will leave this file in case this changes.
 */
namespace MiddleEgyptianDictionary.Services
{
    public static class GardinerConverter
    {
        static Dictionary<string, string> gardinerConverter;

        static GardinerConverter()
        {
            if (gardinerConverter == null)
            {
                gardinerConverter = CreateUnicodeHashSet();
            }
        }

        public static string ConvertGardiner(string input)
        {
            List<string> convertedGlyphs = new List<string>();
            string[] split = input.Split(' ');
            foreach (string glyph in split)
            {
                string fixed_glyph = FixIncongruentLettering(glyph);

                if (gardinerConverter.ContainsKey(fixed_glyph.ToUpper()))
                {
                    convertedGlyphs.Add(gardinerConverter[fixed_glyph.ToUpper()]);
                }
                else
                {
                    convertedGlyphs.Add(" " + fixed_glyph + " ");
                }
            }
            return String.Join("", convertedGlyphs);
        }


        internal static string ConvertMdCToUnicode(string input)
        {
            StringBuilder builder = new StringBuilder();
            int currentIdx = 0;
            char[] separators = new char[] { ' ', '&', ':', '*', '-', '_' };
            string[] split = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            foreach (string glyph in split)
            {
                currentIdx += glyph.Length;
                int i = 1;
                while (currentIdx + i < input.Length && separators.Contains(input.ElementAt(currentIdx + i)))
                    i += 1;
                string separator = currentIdx + i < input.Length ? input.Substring(currentIdx, i) : null;
                currentIdx += i;
                string fixed_glyph = FixIncongruentLettering(glyph);

                if (gardinerConverter.ContainsKey(fixed_glyph.ToUpper()))
                {
                    builder.Append(gardinerConverter[fixed_glyph.ToUpper()]);
                    if (! (separator is null))
                        builder.Append(GetUnicodeSeparator(separator));
                }
                else
                {
                    builder.Append(" " + fixed_glyph + " ");
                    if (!(separator is null))
                        builder.Append(GetUnicodeSeparator(separator));
                }
            }
            return builder.ToString();
        }

        private static Dictionary<string, string> CreateUnicodeHashSet()
        {
            Dictionary<string, string> unicodeTable = new Dictionary<string, string>();
            for (int i = 0x13000; i < 0x1342E; i++)
            {
                string unicodeString = char.ConvertFromUtf32(i);
                string unicodeName = UnicodeInfo.GetName(i).Split(' ').ToList().Last();
                unicodeName = FixUnicodeName(unicodeName);
                unicodeTable.Add(unicodeName, unicodeString);
            }
            return unicodeTable;
        }

        private static string GetUnicodeSeparator(string mdc)
        {
            string answer = "";
            foreach (char c in mdc)
            {
                switch (c)
                {
                    case '(':
                        answer += char.ConvertFromUtf32(0x13437);
                        break;
                    case ')':
                        answer += char.ConvertFromUtf32(0x13438);
                        break;
                    case ':':
                        answer += char.ConvertFromUtf32(0x13430);
                        break;
                    case '*':
                    case '&':
                        answer += char.ConvertFromUtf32(0x13431);
                        break;
                    default:
                        break;
                }
            }
            return answer;
        }
        
        public static string FixIncongruentLettering(string glyph)
        {
            string answer = glyph;
            if (answer.StartsWith("J"))
            {
                answer = "AA" + glyph.Substring(1);
            }
            if (!gardinerConverter.ContainsKey(answer) && answer != "")
            {
                char last = answer.Last();
                if ((last >= 'A' && last <= 'Z') || (last >= 'a' && last <= 'z'))
                {
                    if (gardinerConverter.ContainsKey(answer.Substring(0, answer.Length - 1)))
                    {
                        answer = answer.Substring(0, answer.Length - 1);
                    }
                }
            }
            return answer;
        }

        /// <summary>
        /// Removes trailing zeroes in Unicode Names
        /// </summary>
        /// <param name="unicodeName">Gardiner sign name with excess zeroes</param>
        /// <returns>Gardiner sign name without excess zeroes</returns>
        private static string FixUnicodeName(string unicodeName)
        {
            // [A-Za-z]*[0]*[A-Za-z0-9]* => [A-Za-z]*[A-Za-z0-9]*
            string answer;

            int firstZeroIdx = unicodeName.IndexOf('0');
            if (firstZeroIdx != -1)
            {
                answer = unicodeName.Substring(0, firstZeroIdx);
                if (firstZeroIdx >= 0)
                {
                    for (int i = firstZeroIdx; i < unicodeName.Length; i++)
                    {
                        if (unicodeName[i] != '0')
                        {
                            answer += unicodeName.Substring(i);
                            break;
                        }
                    }
                }
                return answer;
            }
            else
            {
                return unicodeName;
            }
        } 
    }
}

