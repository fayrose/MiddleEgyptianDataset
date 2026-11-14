using MiddleEgyptianDictionary.Parsing;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MiddleEgyptianDictionary.Services
{
    static class MDCTrigramBuilder
    {
        static Dictionary<string, Dictionary<string, int>> Trigrams = new Dictionary<string, Dictionary<string, int>>();

        // Only collected while ExportPreprocessedLines is running
        static List<string> preprocessedLines = null;

        public static Dictionary<string, Dictionary<string, int>> GenerateTrigrams()
        {
            if (Trigrams.Count == 0)
            {
                if (File.Exists(Constants.MDCTrigramsLocation))
                    return GenerateTrigrams(Constants.MDCTrigramsLocation, true);
                else
                    return GenerateTrigrams(Constants.MDCTextsLocation, false, Constants.MDCTrigramsLocation);
            }
            else
            {
                return Trigrams;
            }
        }

        public static Dictionary<String, Dictionary<String, int>> GenerateTrigrams(string path, bool getCached = false, string saveTo = null)
        {
            // If getCached is true, path is location to load from 
            // If getCached is false, path is location to parse from 
            if (getCached)
            {
                Trigrams = (Dictionary<string, Dictionary<string, int>>)Serializer.LoadData(path);
            }
            else
            {
                ParseMDCFiles(path);                
            }
            if (!(saveTo is null))
            {
                Serializer.SaveData(saveTo, Trigrams);
            }
            return Trigrams;
        }

        /// <summary>
        /// Parses the MdC texts at <paramref name="textsPath"/> and writes every preprocessed,
        /// Gardiner-normalized line to <paramref name="saveTo"/> as a JSON array of strings.
        /// This is the input the TransformerFormatter preprocessor expects.
        /// </summary>
        public static int ExportPreprocessedLines(string textsPath, string saveTo)
        {
            preprocessedLines = new List<string>();
            try
            {
                ParseMDCFiles(textsPath);
                File.WriteAllText(saveTo, preprocessedLines.ToJson());
                return preprocessedLines.Count;
            }
            finally
            {
                preprocessedLines = null;
            }
        }

        private static void ParseMDCFiles(string path)
        {
            foreach (string file in Directory.GetFiles(path))
            {
                ParseData(File.ReadAllText(Path.Combine(path, file)));
            }
        }

        private static void ParseData(string fileData)
        {
            fileData = fileData.Replace("\r\n", "\n");
            string[] pages = fileData.Split(new string[] { "!!" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string page in pages)
            {
                ParsePage(page);
            }
        }

        private static void ParsePage(string page)
        {
            string[] lines = page.Split(new string[] { "!\n", "-!\n", "!\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                PreprocessLine(line);
            }
        }

        private static void PreprocessLine(string line)
        {
            String newLine = Regex.Replace(line, "{{([0-9]*,)*[0-9]*}}", "");
            newLine = Regex.Replace(newLine, @"\*\*", "-");
            newLine = Regex.Replace(newLine, @"\\[0-9]*", "");
            newLine = Regex.Replace(newLine, @"\\(\\)?(R?[0-9]+)?", "");
            newLine = Regex.Replace(newLine, @"#1?2?3?4?", "");
            newLine = Regex.Replace(newLine, @"\?[0-9]*", "");
            newLine = Regex.Replace(newLine, "<-(.*)->", "$1");
            newLine = Regex.Replace(newLine, @"\[\[-", "");
            newLine = Regex.Replace(newLine, @"-\]\]", "");
            newLine = Regex.Replace(newLine, @"(\.-)+", "");
            newLine = Regex.Replace(newLine, @"(-\.)+", "-");
            newLine = Regex.Replace(newLine, @"\[&-(.*)-&\]", "$1");
            newLine = Regex.Replace(newLine, @"\[\{-(.*)-\}\]", "$1");
            newLine = Regex.Replace(newLine, @"\[""-(.*)-""\]", "$1");
            newLine = Regex.Replace(newLine, @"\['-(.*)-'\]", "$1");
            newLine = Regex.Replace(newLine, @"([a-zA-Z]+[0-9]+)(&&&)(.*?)(-)", "($1:$3)-");
            newLine = Regex.Replace(newLine, @"(-)(\/\/-)+", "$1");
            newLine = Regex.Replace(newLine,
                @"([a-zA-Z]+[0-9]+|\.\.?)(-|\*|:)(\([a-zA-Z]+[0-9]+)(\^\^\^)([a-zA-Z]+[0-9]+\))(-|\*|:)?",
                "($1$2$3*$5)$6");
            newLine = Regex.Replace(newLine, @"(-|\*|:|&){2,}", "-");
            newLine = Regex.Replace(newLine, @"(-(v|h)\/)+", "");
            newLine = Regex.Replace(newLine, @"((-|\*|:|&)(O|o))+(-|\*|:)", "-");
            newLine = ChangePhoneticToGardiner(newLine);
            newLine = FixNumbering(newLine);
            newLine = newLine.Replace("J", "Aa");
            newLine = newLine.TrimEnd('-', '*', ':', '&');
            var matches = Regex.Matches(newLine, @"(-|\*|:|&)([a-zA-Z]+?)(-|\*|:)");
            Debug.Assert(matches.Count == 0);
            if (!(preprocessedLines is null))
            {
                preprocessedLines.Add(MDCToGardiner.NormalizeGardinerString(newLine));
            }
            ParseLine(newLine);
        }

        private static string ChangePhoneticToGardiner(string line)
        {
            line = Regex.Replace(line, "Y1(v|V)", "Y1A");
            foreach (Match match in Regex.Matches(line, @"(:|-|\*)[a-zA-z]+(:|-|\*)"))
            {
                string matchStr = match.ToString();
                string trimmed = matchStr.Trim(new char[] { ':', '&', '-', '*' });
                string gardiner = MDCToGardiner.GetGardinerFromPhonetics(trimmed);
                if (!(gardiner is null))
                {
                    string replacement = matchStr[0] + gardiner + matchStr[match.Length - 1].ToString();
                    line = Regex.Replace(line, matchStr, replacement);
                }
            }
            return line;
        }

        private static string FixNumbering(string line)
        {
            foreach (Match match in Regex.Matches(line, @"(-|\*|:)([1-5]0?0?|100)(-|\*|:)?"))
            {
                string number = match.Groups[2].ToString();
                string matchStr = match.ToString();
                string replacement = match.Groups[1].ToString() +
                        MDCToGardiner.GetGardinerFromNumbers(number) +
                        match.Groups[3].ToString();
                // Literal replace: the match contains '*', which is a regex metacharacter
                line = line.Replace(matchStr, replacement);
            }
            return line;
        }

        private static void ParseLine(string line)
        {
            // By normalizing the trigrams before building them, the trigrams will
            // be more relevant to dictionary definitions. This will improve formatting.
            int idx = 0;
            String[] splitLine = Regex.Split(line, @"\*|-|:|&");
            for (int i = 0; i < splitLine.Length - 2; i++)
            {
                // Add trigram to dictionary if doesn't presently exist
                string[] normalizedStrArr = new string[] { splitLine[i], splitLine[i + 1], splitLine[i + 2] }
                                                        .Select(x => MDCToGardiner.NormalizeGardinerCode(x))
                                                        .ToArray<string>();
                String joined = String.Join(" ", normalizedStrArr);
                int newIdx = splitLine[i].Length + splitLine[i + 1].Length + splitLine[i + 2].Length + 2;
                if (!Trigrams.ContainsKey(joined))
                {
                    Trigrams.Add(joined, new Dictionary<string, int>());
                }

                // Add MDC value as sub-dictionary with value count
                string mdcValue = MDCToGardiner.NormalizeGardinerString(line.Substring(idx, newIdx));
                if (Trigrams[joined].ContainsKey(mdcValue))
                {
                    Trigrams[joined][mdcValue] += 1;
                }
                else
                {
                    Trigrams[joined].Add(mdcValue, 1);
                }
                idx += splitLine[i].Length + 1;
            }
        }
    }
}
