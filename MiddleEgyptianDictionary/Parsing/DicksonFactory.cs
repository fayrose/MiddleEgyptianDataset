using System.Collections.Generic;
using MiddleEgyptianDictionary.DictionaryParser;
using MiddleEgyptianDictionary.Interfaces;

namespace MiddleEgyptianDictionary.Parsing
{
    class DicksonFactory : IFactory<FileParser>
    {
        public FileParser CreateInstance(Dictionary<string, DictionaryEntry> entryLocation)
        {
            return new DicksonParser(Constants.DicksonDictionaryPdfPath, entryLocation);
        }
    }
}
