using System.Collections.Generic;
using System.IO;
using MiddleEgyptianDictionary.Interfaces;
using MiddleEgyptianDictionary.Parsing;
using MiddleEgyptianDictionary.Services;
using MongoDB.Driver;
using System.Security.Authentication;
using MongoDB.Bson;
using System.Threading.Tasks;
using System;
using System.Linq;

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
            Serializer serializer = new Serializer();
            return serializer.SerializeData(hashTracker.Values);
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

        public static void Main(string[] args)
        {
            Serializer serializer = new Serializer();
            IEnumerable<string> gardinerValues = (IEnumerable<string>)serializer.LoadData("C:\\Users\\lfr2l\\U of T\\NML340\\gardinerSignData.txt");
            MDCTrigramBuilder trigramBuilder = new MDCTrigramBuilder("C:\\Users\\lfr2l\\U of T\\NML340\\Trigrams.txt", true);
            var gardinerToMDC = trigramBuilder.GetFormattedDictionary(gardinerValues.ToList());
            serializer.SaveData("C:\\Users\\lfr2l\\U of T\\NML340\\gardinerToMDC.txt", gardinerToMDC);

        }
    }
}
