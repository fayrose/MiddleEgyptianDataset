using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MiddleEgyptianDictionary.Parsing;

namespace MiddleEgyptianDictionary.Services
{
    public class GardinerFormatter
    {
        GardinerFormatterBuilder Builder = null;
        public GardinerFormatter() { }

        public Dictionary<string, string> GardinerToMdCConverter = new Dictionary<string, string>();

        public Dictionary<string, string> GetConverter(IEnumerable<string> list = null)
        {
            if (File.Exists(Constants.gardinerToMDCLocation))
                return LoadSavedConverter(Constants.gardinerToMDCLocation);
            else if (!(list is null))
                return GetFormattedDictionary(list);
            else if (File.Exists(Constants.gardinerListLocation))
            {
                list = (IEnumerable<string>)Serializer.LoadData(Constants.gardinerListLocation);
                return GetFormattedDictionary(list);
            }
            else
                return null;
        }

        public Dictionary<string, string> GetFormattedDictionary(IEnumerable<string> list, bool saveDict=true)
        {
            Builder = new GardinerFormatterBuilder(MDCTrigramBuilder.GenerateTrigrams());
            Dictionary<string, string> formattedDict = new Dictionary<string, string>();
            foreach (string item in new HashSet<string>(list))
            {
                Debug.WriteLine("Formatting " + (formattedDict.Count + 1).ToString() + " of " + list.Count().ToString());
                string current = item.Replace("  ", " ");
                if (String.IsNullOrWhiteSpace(current) || !Regex.IsMatch(current, @"^(([A-Z]|AA)([0-9])+[A-Z]*\s?)+$"))
                {
                    continue;
                }
                formattedDict.Add(item, Builder.BuildFormattedWordFromTrigrams(current));
            }
            GardinerToMdCConverter = formattedDict;
            if (saveDict)
                SaveConverter(Constants.gardinerToMDCLocation);
            return formattedDict;
        }

        public Dictionary<string, string> LoadSavedConverter(string path)
        {
            GardinerToMdCConverter = (Dictionary<string, string>)Serializer.LoadData(path);
            return GardinerToMdCConverter;
        }

        public void SaveConverter(string path)
        {
            Serializer.SaveData(path, GardinerToMdCConverter);
        }

        public string GetFormattedWord(string word)
        {
            if (GardinerToMdCConverter.TryGetValue(word, out string answer))
            {
                return answer;
            }
            else
            {
                if (Builder is null)
                {
                    Builder = new GardinerFormatterBuilder(MDCTrigramBuilder.GenerateTrigrams());
                }
                return Builder.BuildFormattedWordFromTrigrams(word);
            }
        }

    }
}
