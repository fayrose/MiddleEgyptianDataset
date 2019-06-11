using System;
using System.Collections.Generic;
using System.Linq;
using MiddleEgyptianDictionary;
using MiddleEgyptianDictionary.Models;
using MiddleEgyptianDictionary.Services;

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

    public DictionaryEntry CreateEntry(string transliterationChunk,
                                         string translationChunk,
                                         string posChunk,
                                         string signList,
                                         DataSource dictName)
    {
        
        DictionaryEntry entry;
        // Generate hash string
        string hashString = transliterationChunk + "/" + signList.ToUpper();
        posChunk = PartOfSpeechParser.FixPartOfSpeech(posChunk);
        signList = signList.Replace("  ", " ").ToUpper().Replace("J", "AA");

        // Check if the HashTracker contains the hashstring
        if (!HashTracker.TryGetValue(hashString, out entry))
        {
            string mdc = Formatter.GetFormattedWord(signList);
            string res = ManuelDeCodageToRESConverter.ConvertString(mdc);
            entry = new DictionaryEntry()
            {
                Transliteration = transliterationChunk,
                GardinerSigns = signList,
                ManuelDeCodage = mdc,
                Res = res.Equals(mdc) ? null : res
            };
            HashTracker.Add(hashString, entry);
        }

        Translation currentTranslation = entry.Translations.Where(x => x.translation == translationChunk.Trim()).FirstOrDefault();
        if (currentTranslation == default(Translation))
        {
            currentTranslation = new Translation() { translation = translationChunk.Trim() };
            entry.Translations.Add(currentTranslation);
        }
        Metadata existingMetadata = currentTranslation.TranslationMetadata;
        bool notDuplicate = existingMetadata != null && 
                            existingMetadata.DictionaryName == dictName &&
                            existingMetadata.PartOfSpeech == posChunk ?
                            false : true;
        if (notDuplicate)
        {
            Metadata metadata = new Metadata() { DictionaryName = dictName };
            currentTranslation.TranslationMetadata = metadata;
            if (posChunk != "" && posChunk != null)
            {
                metadata.PartOfSpeech = posChunk.Trim();
            }
        }
        return entry;

    }
}
