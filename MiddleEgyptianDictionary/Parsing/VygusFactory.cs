using MiddleEgyptianDictionary.DictionaryParser;
using MiddleEgyptianDictionary.Interfaces;
using System.Collections.Generic;

namespace MiddleEgyptianDictionary.Parsing
{
    class VygusFactory : IFactory<FileParser>
    {
        public FileParser CreateInstance(Dictionary<string, DictionaryEntry> entryLocation)
        {
            return Create2018Instance(entryLocation);
        }

        public FileParser Create2012Instance(Dictionary<string, DictionaryEntry> entryLocation)
        {
            return new VygusParser(Constants.VygusDictionary2012PdfPath, entryLocation);
        }

        public FileParser Create2018Instance(Dictionary<string, DictionaryEntry> entryLocation)
        {
            return new VygusParser(Constants.VygusDictionary2018PdfPath, entryLocation, is2018:true);
        }


    }
}
