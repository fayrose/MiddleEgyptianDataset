using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MiddleEgyptianDictionary.DictionaryParser
{
    public class VygusParser : FileParser
    {
        private readonly int startPage, endPage;
        private readonly bool is2018;
        private readonly Dictionary<int, Dictionary<int, string>> correctlyFormattedDict;
        public VygusParser(string pdfName,
                           Dictionary<string, DictionaryEntry> hashTracker,
                           bool is2018 = false,
                           string pathToCorrectlyFormattedJson = null) :
            base(pdfName, hashTracker)
        {
            startPage = is2018 ? 1 : Constants.Vygus2012FirstPage;
            endPage = is2018 ? Constants.Vygus2018LastPage : Constants.Vygus2012LastPage;
            this.is2018 = is2018;
            if (is2018 && !(pathToCorrectlyFormattedJson is null))
            {
                using StreamReader file = File.OpenText(pathToCorrectlyFormattedJson);
                JsonSerializer js = new JsonSerializer();
                correctlyFormattedDict = (Dictionary<int, Dictionary<int, string>>)js.Deserialize(file, typeof(Dictionary<int, Dictionary<int, string>>));
            }
        }

        public override void ParseAll()
        {
            using (PdfReader reader = new PdfReader(fileName))
            using (PdfDocument pdf = new PdfDocument(reader))
            {
                for (int i = startPage; i <= endPage; i++)
                {
                    Debug.WriteLine("Page " + i.ToString());
                    ParsePageHelper(i, reader, pdf);
                }
            }
        }

        private void ParsePageHelper(int pageNumber, PdfReader reader, PdfDocument doc)
        {
            String text = PdfTextExtractor.GetTextFromPage(doc.GetPage(pageNumber), new LocationTextExtractionStrategy());
            VygusAssociateDefinitions(VygusFilterText(text), pageNumber);
        }

        public void VygusParsePage(int pageNumber)
        {
            using (PdfReader reader = new PdfReader(fileName))
            using (PdfDocument pdf = new PdfDocument(reader))
            {
                String text = PdfTextExtractor.GetTextFromPage(pdf.GetPage(pageNumber), new LocationTextExtractionStrategy());
                VygusAssociateDefinitions(VygusFilterText(text), pageNumber);
            }
        }

        public string VygusFilterText(string rawData)
        {
            if (is2018)
                return rawData;
            StringBuilder newText = new StringBuilder();
            List<string> wordList = rawData.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            for (int i = 0; i < wordList.Count; i++)
            {
                if (wordList[i].Contains("vygus"))
                {
                    wordList[i] = "\n";
                }
            }
            string reconstructed = String.Join(" ", wordList);
            return reconstructed;
        }

        public List<DictionaryEntry> VygusAssociateDefinitions(string reconstructed, int pageNumber)
        {
            List<string> wordList = reconstructed.Split('\n').ToList();
            List<DictionaryEntry> dictList = new List<DictionaryEntry>();
            int[] pageList = is2018 ? new int[] { } : new int[] { 1560, 1658 };
            if (!pageList.Contains(pageNumber))
            {
                wordList.RemoveAt(wordList.Count - 1);
            }

            if (wordList.Count % 2 == 1)
            {
                for (int k = 0; k < (wordList.Count - 1); k++)
                {
                    if (wordList[k].Contains('[') && wordList[k+1].Contains('['))
                    {
                        wordList.Insert((k + 1), "?");
                        break;
                    }
                    bool long_def = wordList[k].Contains('-') && wordList[k + 1].Contains('-');
                    long_def |= (wordList[k].Contains("(") && wordList[k+1].Contains(')'));
                    if (long_def)
                    {
                        wordList[k] = wordList[k] + wordList[k+1];
                        wordList.RemoveAt(k+1);
                        break;
                    }
                }
            }
            Debug.Assert(wordList.Count % 2 == 0);
            int halfCount = wordList.Count / 2;
            for (int j = 0; j < halfCount; j++)
            {
                string formatting = null;
                if (this.correctlyFormattedDict.ContainsKey(pageNumber))
                {
                    this.correctlyFormattedDict[pageNumber].TryGetValue(j, out formatting);
                }
                    
                ParseWordDataRegex(wordList[j * 2].Trim(), wordList[j * 2 + 1].Trim(), pageNumber, j, formatting);
            }

            return dictList;
        }
        
        private void ParseWordDataRegex(string wordData, string transliteration, int pageNumber, int indexOnPage, string formatting = null)
        {
            string partOfSpeech = "", translation = "", signString = "";

            MatchCollection inBrackets = Regex.Matches(wordData, @"(\[((\w|\s|-|\.|'|\?)+)\])");
            MatchCollection inCurly = Regex.Matches(wordData, @"(\{((\w|\s|-|\.|'|\?)+)\})");
            MatchCollection inParen = Regex.Matches(wordData, @"(\(((\w|\s|-|\.|'|\?|,|/|&)+)\))");
            MatchCollection CurlytoParen = Regex.Matches(wordData, @"(\{((\w|\s|-|\.|'|\?)+)\))");
            MatchCollection BracketToParen = Regex.Matches(wordData, @"(\[((\w|\s|-|\.|'|\?)+)\))");
            MatchCollection CurlyToApos = Regex.Matches(wordData, @"(\{(((\w|\s|-|\.|\?)+)\')+)");
            MatchCollection SplitSpacesToBracket = Regex.Matches(wordData, @"((\s\s(\s?))((\w|\s|-|\.|'|\?)+)\])");

            int[] startCollection = new int[] { wordData.LastIndexOfAny(new char[] {'(' , '['}),
                                                wordData.LastIndexOf("   "),
                                                wordData.LastIndexOf("  ") }
                                        .Where(x => x > -1 ).ToArray();
            int[] endCollection = new int[] { wordData.LastIndexOfAny(new char[] { '}', ')', '\'', ']' }),
                                              wordData.LastIndexOf("  "),
                                              wordData.LastIndexOf("   ")};

            int startIndex = startCollection.Min();
            int endIndex = endCollection.Max();

            Debug.Assert(startIndex > -1);
            translation = wordData.Substring(0, startIndex);

            Debug.Assert(endIndex > -1);
            signString = wordData.Substring(endIndex + 1);

            int posIndex = ChoosePartOfSpeech(wordData, inBrackets);
            if (posIndex != -1)
            {
                partOfSpeech = inBrackets[posIndex].Groups[2].Value;
                for (int i = 0; i < inBrackets.Count; i++)
                {
                    if (i != posIndex && inBrackets[i].Index > startIndex)
                    {
                        translation += String.IsNullOrWhiteSpace(translation.LastOrDefault().ToString()) ? "" : " ";
                        translation += inBrackets[i];
                    }
                }
            }
            
            foreach (Match item in inCurly)
            {
                if (item.Index >= startIndex)
                {
                    translation += String.IsNullOrWhiteSpace(translation.LastOrDefault().ToString()) ? "" : " ";
                    translation += item;
                }
            }

            foreach (Match item in inParen)
            {
                if (item.Index >= startIndex)
                {
                    translation += String.IsNullOrWhiteSpace(translation.LastOrDefault().ToString()) ? "" : " ";
                    translation += item;
                }
            }

            foreach (Match item in CurlytoParen)
            {
                if (item.Index >= startIndex)
                {
                    translation += String.IsNullOrWhiteSpace(translation.LastOrDefault().ToString()) ? "" : " ";
                    translation += item.ToString().Substring(0, item.Length - 1) + '}';
                }
            }

            foreach (Match item in BracketToParen)
            {
                if (item.Index >= startIndex)
                {
                    translation += String.IsNullOrWhiteSpace(translation.LastOrDefault().ToString()) ? "" : " ";
                    translation += item.ToString().Substring(0, item.Length - 1) + ']';
                }
            }

            foreach (Match item in CurlyToApos)
            {
                if (item.Index >= startIndex && wordData.IndexOf('}') == -1)
                {
                    translation += String.IsNullOrWhiteSpace(translation.LastOrDefault().ToString()) ? "" : " ";
                    translation += item;
                    translation += '}';
                }
            }

            foreach (Match item in SplitSpacesToBracket)
            {
                if (item.Index >= startIndex && wordData.IndexOf('[') == -1) // TODO : Add condition where not in [ .. ]
                {
                    translation += String.IsNullOrWhiteSpace(translation.LastOrDefault().ToString()) ? "" : " ";
                    translation += item;
                }
            }
            
            // Remove extraneous whitespace in string
            signString = String.Join(" ", signString.Split(new string[] { " - ", "-", " " }, StringSplitOptions.RemoveEmptyEntries));
            transliteration = String.Join(" ", transliteration.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            partOfSpeech = String.Join(" ", partOfSpeech.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            translation = String.Join(" ", translation.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            
            transliteration = transliteration.Replace("xn,m", "xnm");
            transliteration = transliteration.Replace(".", " ");
            transliteration = transliteration.Replace("-", " ");
            transliteration = transliteration.Replace("  ", " ");
            transliteration = transliteration.Replace("(", string.Empty).Replace(")", string.Empty);
            transliteration = transliteration.Replace("[", string.Empty).Replace("]", string.Empty);
            transliteration = transliteration.Replace("{", string.Empty).Replace("}", string.Empty);
            transliteration = transliteration.Trim();
            
            var splitTranslit = transliteration.Split('/');
            if (splitTranslit.Length > 1)
            {
                foreach (var translitOption in splitTranslit)
                {
                    CreateEntry(translitOption.Trim(),
                        translation.Trim(),
                        partOfSpeech.Trim(),
                        signString.Trim(),
                        DataSource.vygus,
                        pageNumber,
                        indexOnPage,
                        formatting);
                }
            }
            else
            {
                CreateEntry(transliteration.Trim(),
                translation.Trim(),
                partOfSpeech.Trim(),
                signString.Trim(),
                DataSource.vygus,
                pageNumber,
                indexOnPage,
                formatting);
            }
        }

        private int ChoosePartOfSpeech(string wordData, MatchCollection inBrackets)
        {
            switch (inBrackets.Count)
            {
                case 0:
                    return -1;
                case 1:
                    return 0;
                case int count when count > 1:
                    for (int i = 0; i < count; i++)
                    {
                        string pos = inBrackets[i].Groups[2].Value;
                        if (pos.Contains(Constants.Verb) || pos.Contains(Constants.Noun))
                        {
                            return i;
                        }
                    }
                    return count - 1;
                default:
                    return -1;
            }
           

        }
    }
}

