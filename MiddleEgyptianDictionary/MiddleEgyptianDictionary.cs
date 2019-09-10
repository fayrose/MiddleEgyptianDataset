using System.Collections.Generic;
using System.IO;
using System.Linq;
using MiddleEgyptianDictionary.Interfaces;
using MiddleEgyptianDictionary.Parsing;
using MiddleEgyptianDictionary.Services;
using MongoDB.Bson;

namespace MiddleEgyptianDictionary
{
    public class MiddleEgyptianDictionary
    {
        Dictionary<string, DictionaryEntry> hashTracker = new Dictionary<string, DictionaryEntry>();

        public MiddleEgyptianDictionary()
        {
        }

        public void CreateDictionaries()
        {
            IFactory<FileParser>[] factories = { new DicksonFactory(),
                                                 new LexiconFactory(),
                                                 new VygusFactory() };
            foreach (var factory in factories)
            {
                FileParser parser = factory.CreateInstance(hashTracker);
                parser.ParseAll();
            }
        }

        public IEnumerable<BsonDocument> SerializeDictionaries()
        {
            return Serializer.DataToBSon(hashTracker.Values);
        }

        public void WriteDictionaries(string path)
        {
            using (FileStream stream = File.OpenWrite(path))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(SerializeDictionaries());
                }
            }
        }

        public IEnumerable<DictionaryEntry> GetEntries()
        {
            return hashTracker.Values.ToArray();
        }

        public static void Main(string[] args)
        {
        }
    }
}
