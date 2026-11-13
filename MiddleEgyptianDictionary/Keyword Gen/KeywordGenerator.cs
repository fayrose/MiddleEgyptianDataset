using HtmlAgilityPack;
using MiddleEgyptianDictionary.Models;
using MiddleEgyptianDictionary.Properties;
using Porter2Stemmer;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace MiddleEgyptianDictionary
{
    public class KeywordGenerator
    {
        private readonly ImmutableArray<string> StopWords;
        Dictionary<string, KeywordSearch> KeywordTable;
        private readonly EnglishPorter2Stemmer Stemmer;

        public KeywordGenerator()
        {
            StopWords = GetStopWords();
            KeywordTable = new Dictionary<string, KeywordSearch>();
            Stemmer = new EnglishPorter2Stemmer();
        }

        private ImmutableArray<string> GetStopWords()
        {
            return Resources.StopWords
                            .Replace("\r", "")
                            .Split('\n')
                            .Select(x => x.Trim())
                            .ToImmutableArray();
        }

        public void GenerateKeywordsFromEntries(IEnumerable<DictionaryEntry> entries)
        {
            var entriesArr = entries.ToArray();
            EnglishPorter2Stemmer stemmer = new EnglishPorter2Stemmer();
            int count = entriesArr.Count();
            for (int j = 0; j < count; j++)
            {
                DictionaryEntry entry = entriesArr[j];
                Console.WriteLine("Entry " + (j + 1).ToString() + " of " + count.ToString());
                foreach (Translation translationObj in entry.Translations)
                {
                    var translationStr = translationObj.translation;
                    if (translationObj.TranslationMetadata.Select(x => x.DictionaryName).Contains(DataSource.faulkner))
                    {
                        var doc = new HtmlDocument();
                        var sanitizedText = Regex.Replace(translationStr, "<canvas class=\"res\">.+</canvas>", "");
                        sanitizedText = Regex.Replace(sanitizedText, "<i><span style=\"color: #0070BF\">.+</i></span>", "");
                        sanitizedText = Regex.Replace(sanitizedText, "<sup>.+</sup>", "");
                        
                        doc.LoadHtml(sanitizedText);

                        // Don't use definition numbers as a part of keywords
                        translationStr = Regex.Replace(doc.DocumentNode.InnerText, @"\n?\d+\)", " ").Trim();
                        translationStr = translationStr.Replace("\n", " ").Replace("§", "");
                    }

                    var withoutStopWords = SanitizeSearchInput(translationStr, translationObj.TranslationMetadata.Select(x => x.PartOfSpeech).ToArray());
                    Debug.Assert(withoutStopWords.Length > 0);
                    for (int i = 0; i < withoutStopWords.Count(); i++)
                    {
                        if (KeywordTable.ContainsKey(withoutStopWords[i]))
                        {
                            KeywordTable[withoutStopWords[i]].AddIdToEntryIds(entry.Id);
                        }
                        else
                        {
                            KeywordTable.Add(withoutStopWords[i], new KeywordSearch(withoutStopWords[i], entry.Id));
                        }
                    }
                }
            }
        }

        public string[] SanitizeSearchInput(string search, string[] partOfSpeech)
        {
            var sanitized = string.Join("", search.Select(x => char.IsPunctuation(x) && x != '\'' ? ' ' : char.ToLower(x)))
                                                                              .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var withoutStopWords = sanitized.Except(StopWords).ToArray();

            // If there are no keywords once the stop words are removed, back off to the StopWords. If that too is empty, back off to the translation.
            bool withoutStopWordsIsEmpty = withoutStopWords.Count() == 0;
            if (withoutStopWordsIsEmpty && sanitized.Count() == 0)
            {
                withoutStopWords = search.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }
            else if (withoutStopWordsIsEmpty)
            {
                withoutStopWords = sanitized;
            }

            // If neither of these plan B's work, back off to the Part of Speech, should one exist.
            if (withoutStopWords.Count() == 0 && partOfSpeech != null)
            {
                HashSet<string> posSet = new HashSet<string>();
                foreach (var s in partOfSpeech)
                {
                    foreach (var sub in s.Split())
                    {
                        posSet.Add(sub);
                    }
                }
                withoutStopWords = posSet.ToArray();
            }
            return withoutStopWords.Select(x => Stemmer.Stem(x.ToLower()).Value.ToLower()).ToArray();
        }

        public IEnumerable<KeywordSearch> GetKeywordSearchList()
        {
            return KeywordTable.Values;
        }
    }
}
