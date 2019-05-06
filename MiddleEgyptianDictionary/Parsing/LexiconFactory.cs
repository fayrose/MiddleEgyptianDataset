using MiddleEgyptianDictionary.DictionaryParser;
using MiddleEgyptianDictionary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiddleEgyptianDictionary.Parsing
{
    class LexiconFactory : IFactory<FileParser>
    {
        public FileParser CreateInstance(Dictionary<string, DictionaryEntry> entryLocation)
        {
            return new LexiconParser(Constants.LexiconDictionaryPdfPath, entryLocation);
        }
    }
}
