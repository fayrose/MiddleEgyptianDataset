using System;
using System.Collections.Generic;
using System.Linq;
using MiddleEgyptianDictionary;
using MiddleEgyptianDictionary.Models;
using MiddleEgyptianDictionary.Services;
using MongoDB.Bson;

public abstract class FileParser
{
    internal string fileName;
    internal Dictionary<string, DictionaryEntry> HashTracker;
    GardinerFormatter Formatter;

    public FileParser(string fileName, Dictionary<string, DictionaryEntry> hashTracker)
    {
        this.fileName = fileName;
        HashTracker = hashTracker;
        Formatter = new GardinerFormatter();
        Dictionary<string, string> converter = Formatter.GetConverter();
    }

    public abstract void ParseAll();

    public void CreateEntry(string transliterationChunk,
                                         string translationChunk,
                                         string posChunk,
                                         string signList,
                                         DataSource dictName,
                                         int? page = null,
                                         int? indexOnPage = null,
                                         string formatting = null,
                                         DictionaryEntry baseEntry = null)
    {
        
        DictionaryEntry entry;
        // Generate hash string
        string hashString = transliterationChunk + "/" + signList.ToUpper();
        posChunk = PartOfSpeechParser.FixPartOfSpeech(posChunk);
        signList = signList.Replace("  ", " ").ToUpper().Replace("J", "AA");

        // Check if the HashTracker contains the hashstring
        if (!HashTracker.TryGetValue(hashString, out entry))
        {
            string mdc = formatting ?? Formatter.GetFormattedWord(signList);
            string res = ManuelDeCodageToRESConverter.ConvertString(mdc);
            if (baseEntry is null)
            {
                entry = new DictionaryEntry()
                {
                    Id = ObjectId.GenerateNewId(),
                    Transliteration = transliterationChunk,
                    GardinerSigns = signList,
                    ManuelDeCodage = mdc.Replace("AA", "Aa").Replace("J", "Aa"),
                    Res = res.Equals(mdc) ? null : res.Replace("AA", "Aa").Replace("J", "Aa"),
                    IsCorrectlyFormatted = !(formatting is null)
                };
            }
            else
            {
                entry = baseEntry;
                entry.ManuelDeCodage = mdc.Replace("AA", "Aa");
                entry.Res = res.Equals(mdc) ? null : res.Replace("AA", "Aa");
            }
            
            HashTracker.Add(hashString, entry);
        }
        if (!entry.IsCorrectlyFormatted && !(formatting is null) && entry.ManuelDeCodage != formatting)
        {
            entry.ManuelDeCodage = formatting.Replace("AA", "Aa").Replace("J", "Aa");
            string res = ManuelDeCodageToRESConverter.ConvertString(formatting);
            entry.Res = res.Equals(formatting) ? null : res.Replace("AA", "Aa").Replace("J", "Aa");
            entry.IsCorrectlyFormatted = true;
        }
        translationChunk = translationChunk.Replace("''", "'").Trim();
        Translation currentTranslation = entry.Translations.Where(x => x.translation == translationChunk).FirstOrDefault();
        if (currentTranslation == default(Translation))
        {
            bool appendedToExistingEntry = false;
            if (string.IsNullOrWhiteSpace(translationChunk))
            {
                currentTranslation = entry.Translations.Where(x => x.TranslationMetadata.All(x => x.PartOfSpeech is null)).FirstOrDefault();
                if (!(currentTranslation is null))
                {
                    appendedToExistingEntry = true;
                }
            }
            if (!appendedToExistingEntry)
            {
                currentTranslation = new Translation() { translation = translationChunk };
                entry.Translations.Add(currentTranslation);
            }
        }
        Metadata[] existingMetadata = currentTranslation.TranslationMetadata;
        bool notDuplicate = true;
        if (existingMetadata != null)
        {
            foreach (var m in existingMetadata)
            {
                if (m.DictionaryName == dictName && m.PartOfSpeech == posChunk && m.Page == page && m.IndexOnPage == indexOnPage)
                {
                    notDuplicate = false;
                }
            }
        }

        if (notDuplicate)
        {
            Metadata metadata = new Metadata() { DictionaryName = dictName, Page = page, IndexOnPage = indexOnPage };

            if (currentTranslation.TranslationMetadata is null)
            {
                currentTranslation.TranslationMetadata = new Metadata[] { metadata };
            }
            else
            {
                currentTranslation.TranslationMetadata = currentTranslation.TranslationMetadata.Concat(new Metadata[] { metadata }).ToArray();
            }

            if (posChunk != "" && posChunk != null)
            {
                metadata.PartOfSpeech = posChunk.Trim();
            }
        }
    }
}
