using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MiddleEgyptianDictionary.Interfaces;
using MiddleEgyptianDictionary.Parsing;
using MiddleEgyptianDictionary.Services;
using MongoDB.Bson;
using Newtonsoft.Json;

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
            IFactory<FileParser>[] factories = { new FaulknerFactory(),
                                                 new VygusFactory(), 
                                                 new DicksonFactory(),
                                                 new LexiconFactory() };
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

        public DictionaryEntry[] GetEntries()
        {
            return hashTracker.Values.ToArray();
        }

        public T[][] Partition<T>(IEnumerable<T> array, int chunkSize)
        {
            return array.Select((s, i) => array.Skip(i * chunkSize).Take(chunkSize).ToArray()).Where(a => a.Any()).ToArray();
        }

        public static void Main()
        {
            // Slowly add dictionary entries to prevent going over max RUs/s
            var med = new MiddleEgyptianDictionary();
            med.CreateDictionaries();
            var entries = med.GetEntries();
            /*var chunkSize = 75;
            var chunkedEntries = med.Partition<DictionaryEntry>(entries, chunkSize);

            DbManager manager = new DbManager();
            
            KeywordGenerator keywordGenerator = new KeywordGenerator();
            keywordGenerator.GenerateKeywordsFromEntries(med.GetEntries());

            var chunkedKeywords = med.Partition(keywordGenerator.GetKeywordSearchList(), chunkSize);

            manager.DropEntryCollection();
            manager.DropKeywordCollection();

            for (int i = 0; i < chunkedEntries.Length; i++)
            {
                var task1 = Task.Run(async () => { await manager.WriteEntriesToDbAsync(chunkedEntries[i]); });
                task1.Wait();
                Thread.Sleep(1000);
                Console.WriteLine($"Successfully wrote entry partition {i} of {chunkedEntries.Length} to db.");
            }

            for (int i = 0; i < chunkedKeywords.Length; i++)
            {
                var task1 = Task.Run(async () => { await manager.WriteKeywordsToDbAsync(chunkedKeywords[i]); });
                task1.Wait();
                Thread.Sleep(1000);
                Console.WriteLine($"Successfully wrote keyword partition {i} of {chunkedKeywords.Length} to db.");
            }
            */
            var withPunc = entries.Where(x => x.Transliteration.Any(y => char.IsPunctuation(y) && y != '-')).ToArray();
            Console.WriteLine("Now what?");
        }
    }
}
