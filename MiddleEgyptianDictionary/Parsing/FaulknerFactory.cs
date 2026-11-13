using System.Collections.Generic;
using MiddleEgyptianDictionary.DictionaryParser;
using MiddleEgyptianDictionary.Interfaces;

namespace MiddleEgyptianDictionary.Parsing
{
    class FaulknerFactory : IFactory<FileParser>
    {
        public FileParser CreateInstance(Dictionary<string, DictionaryEntry> entryLocation)
        {
            return new FaulknerParser(Constants.FaulknerDictionaryPdfPath, entryLocation);
        }
    }
}
