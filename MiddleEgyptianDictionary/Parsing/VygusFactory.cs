using MiddleEgyptianDictionary.DictionaryParser;
using MiddleEgyptianDictionary.Interfaces;
using System.Collections.Generic;

namespace MiddleEgyptianDictionary.Parsing
{
    class VygusFactory : IFactory<FileParser>
    {
        public FileParser CreateInstance(Dictionary<string, DictionaryEntry> entryLocation)
        {
            return new VygusParser(Constants.VygusDictionaryPdfPath, entryLocation);
        }
    }
}
