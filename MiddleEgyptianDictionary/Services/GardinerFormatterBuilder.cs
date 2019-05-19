using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace MiddleEgyptianDictionary.Services
{
    class GardinerFormatterBuilder
    {
        Dictionary<string, Dictionary<string, int>> Trigrams;
        public GardinerFormatterBuilder(Dictionary<string, Dictionary<string, int>> trigrams)
        {
            Trigrams = trigrams;
        }

        public string BuildFormattedWordFromTrigrams(string wordGardiner)
        {
            string[] splitWord = wordGardiner.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (splitWord.Length < 2)
                return wordGardiner;
            else if (splitWord.Length == 2)
            {
                List<string> triKeys = Trigrams.Keys.ToList().Where(x => x.Contains(wordGardiner)).ToList();
                if (triKeys.Count == 0)
                {
                    return String.Join("-", splitWord);
                }
                else
                {
                    Dictionary<string, int> possibilities = new Dictionary<string, int>();
                    foreach (string key in triKeys)
                    {
                        var possDic = Get2Possibility(Trigrams[key], splitWord);
                        possibilities = ConcatDicts(possibilities, possDic);
                    }
                    if (possibilities.Count == 1)
                    {
                        return String.Join(possibilities.Keys.SingleOrDefault(), splitWord);
                    }
                    else
                    {
                        return String.Join(ChooseBestPossibility(possibilities), splitWord);
                    }
                }
            }
            else
            {
                List<string> trigramPossibilities = new List<string>();
                for (int i = 0; i < splitWord.Length - 2; i++)
                {
                    string trigram = String.Join(" ", new string[] { splitWord[i], splitWord[i + 1], splitWord[i + 2] });
                    if (Trigrams.TryGetValue(trigram, out Dictionary<string, int> possibilities))
                    {
                        trigramPossibilities.Add(ChooseBestPossibility(possibilities));
                    }
                    else
                    {
                        // Back off to bigrams (Katz anyone?)
                        string bi1 = splitWord[i] + " " + splitWord[i + 1];
                        string bi2 = splitWord[i + 1] + " " + splitWord[i + 2];
                        string formattedBi1 = BuildFormattedWordFromTrigrams(bi1);
                        string formattedBi2 = BuildFormattedWordFromTrigrams(bi2);
                        trigramPossibilities.Add(CombineBigrams(formattedBi1, formattedBi2));
                    }
                }
                return CombineTrigrams(trigramPossibilities, wordGardiner);
            }
        }

        private string CombineTrigrams(List<string> trigramPossibilities, string toMake)
        {
            Debug.Assert(trigramPossibilities.Count != 0);
            if (trigramPossibilities.Count == 1)
            {
                return trigramPossibilities.First();
            }
            else
            {
                string current = trigramPossibilities[0];
                for (int i = 1; i < trigramPossibilities.Count; i++)
                {
                    current = CombineBigrams(current, trigramPossibilities[i]);
                }
                return current;
            }
        }

        private Dictionary<string, int> ConcatDicts(Dictionary<string, int> arr1, Dictionary<string, int> arr2)
        {
            foreach (string middle in arr2.Keys.ToArray())
            {
                if (arr1.ContainsKey(middle))
                    arr1[middle] += arr2[middle];
                else
                    arr1.Add(middle, arr2[middle]);
            }
            return arr1;
        }

        private string ChooseBestPossibility(Dictionary<string, int> possibilities)
        {
            int maxValue = possibilities.Values.Max();
            List<string> keysWithMaxValue = possibilities.Keys.Where(x => possibilities[x] == maxValue).ToList();
            if (keysWithMaxValue.Count > 1)
            {
                var ans = possibilities.Select(x => x.Key.Count(y => y == '-')).Min();
                string middle = possibilities.Where(x => x.Key.Count(y => y == '-') == ans).First().Key;
                return middle;

            }
            return keysWithMaxValue.First();
        }

        private string CombineBigrams(string formattedBi1, string formattedBi2)
        {
            // Knuth Morris Pratt Main Alg
            //https://www.ics.uci.edu/~eppstein/161/960227.html

            string untouched = formattedBi1;
            if (formattedBi1.Length > formattedBi2.Length)
            {
                formattedBi1 = formattedBi1.Substring(formattedBi1.Length - formattedBi2.Length);
            }
            int[] T = KnuthMorrisPrattDynArray(formattedBi2);
            int m = 0;
            int i = 0;
            while (m + i < formattedBi1.Length)
            {
                if (formattedBi2[i] == formattedBi1[m + i])
                {
                    i += 1;
                }
                else
                {
                    m += i - T[i];
                    if (i > 0) i = T[i];
                }
            }
            if (i != 0)
            {
                return untouched.Substring(0, untouched.Length - i) + formattedBi2;
            }
            else
            {
                // Fix connection
                int lastPunctuationIdx = formattedBi2.LastIndexOfAny(new char[] { '&', '-', ':', '*' });
                return untouched + formattedBi2.Substring(lastPunctuationIdx);
            }
        }

        private int[] KnuthMorrisPrattDynArray(string s)
        {
            // Knuth Morris Pratt Dynamic Array gen
            var T = new int[s.Length];
            int cnd = 0;
            T[0] = -1;
            T[1] = 0;
            int pos = 2;
            while (pos < s.Length)
            {
                if (s[pos - 1] == s[cnd])
                {
                    T[pos] = cnd + 1;
                    pos += 1;
                    cnd += 1;
                }
                else if (cnd > 0)
                {
                    cnd = T[cnd];
                }
                else
                {
                    T[pos] = 0;
                    pos += 1;
                }
            }
            return T;
        }

        private Dictionary<string, int> Get2Possibility(Dictionary<string, int> dictionary, string[] splitWord)
        {
            Dictionary<string, int> middleCount = new Dictionary<string, int>();
            foreach (var item in dictionary.Keys)
            {
                Match match = Regex.Match(item, splitWord[0] + @"((-|\*|:|&)_?)" + splitWord[1]);
                string middle = match.Groups[1].ToString();
                int multiplier = 1;
                if (middle.Contains('_'))
                {
                    Console.WriteLine("Found it!");
                }
                if (middleCount.ContainsKey(middle))
                    middleCount[middle] += 1 * multiplier;
                else
                    middleCount.Add(middle, 1 * multiplier);
            }
            return middleCount;
        }

    }
}
