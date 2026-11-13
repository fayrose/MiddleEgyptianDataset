using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using MiddleEgyptianDictionary.Models;
using Newtonsoft.Json;

namespace MiddleEgyptianDictionary.Parsing
{
    class FaulknerParser : FileParser
    {
        public FaulknerParser(string fileName, Dictionary<string, DictionaryEntry> hashTracker) : base(fileName, hashTracker) 
        {
        }

        public override void ParseAll()
        {
            var entryStr = File.ReadAllText(Constants.FaulknerEntriesPath);
            var entries = JsonConvert.DeserializeObject<DictionaryEntry[]>(entryStr);
            foreach (var entry in entries)
            {
                entry.Id = MongoDB.Bson.ObjectId.GenerateNewId();
                entry.IsCorrectlyFormatted = true;
                Debug.Assert(entry.Translations.Count == 1);

                var firstTranslation = entry.Translations.ToArray()[0];
                firstTranslation.TranslationMetadata[0].DictionaryName = DataSource.faulkner;

                CreateEntry(entry.Transliteration.Trim(),
                            firstTranslation.translation,
                            null,
                            entry.GardinerSigns,
                            DataSource.faulkner,
                            firstTranslation.TranslationMetadata[0].Page,
                            firstTranslation.TranslationMetadata[0].IndexOnPage,
                            entry.ManuelDeCodage,
                            entry);
            }
        }
    }
}
