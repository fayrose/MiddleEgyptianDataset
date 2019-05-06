using System;
using System.Collections.Generic;
using System.Linq;
using MiddleEgyptianDictionary;
using MiddleEgyptianDictionary.Models;

public abstract class FileParser
{
    internal string fileName;
    internal Dictionary<string, DictionaryEntry> HashTracker;

    public FileParser(string fileName, Dictionary<string, DictionaryEntry> hashTracker)
    {
        this.fileName = fileName;
        HashTracker = hashTracker;
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
        string hashString = transliterationChunk.Trim() + "/" + signList.Trim().ToUpper();
        posChunk = FixPartOfSpeech(posChunk);
        // Check if the HashTracker contains the hashstring
        if (!HashTracker.TryGetValue(hashString, out entry))
        {
            entry = new DictionaryEntry()
            {
                transliteration = transliterationChunk.Trim(),
                gardinerSigns = signList.Trim().ToUpper()
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

    private string FixPartOfSpeech(string posString)
    {
        if (posString == null)
            return null;
        string fixedPoS = posString.Replace(" n.", "noun");
        fixedPoS = posString.Replace("v.", "verb");
        fixedPoS = fixedPoS.Replace("adj.", "adjective");
        fixedPoS = fixedPoS.Replace("prn.", "pronoun");
        return fixedPoS;
    }

}
