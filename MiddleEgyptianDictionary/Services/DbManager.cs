using MiddleEgyptianDictionary.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Linq;

namespace MiddleEgyptianDictionary.Services
{
    public class DbManager
    {
        MongoClient _client;
        public DbManager()
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(
                          new MongoUrl(Constants.ConnectionString)
                        );

              new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            _client = new MongoClient(settings);
        }

        public async Task WriteEntriesToDbAsync(IEnumerable<DictionaryEntry> entries)
        {
            IEnumerable<WriteModel<DictionaryEntry>> wrapped = entries.Select(x => new InsertOneModel<DictionaryEntry>(x));
            var _db = _client.GetDatabase("MiddleEgyptianDictionary");
            // _db.DropCollection("Entries");
            var entryCollection = _db.GetCollection<DictionaryEntry>("Entries");
            await entryCollection.BulkWriteAsync(wrapped);
        }

        public async Task WriteKeywordsToDbAsync(IEnumerable<KeywordSearch> keywords)
        {
            IEnumerable<WriteModel<KeywordSearch>> wrapped = keywords.Select(x => new InsertOneModel<KeywordSearch>(x));
            var _db = _client.GetDatabase("MiddleEgyptianDictionary");
            // _db.DropCollection("Keywords");
            var entryCollection = _db.GetCollection<KeywordSearch>("Keywords");
            await entryCollection.BulkWriteAsync(wrapped);
        }

        public IMongoCollection<DictionaryEntry> GetExistingDbEntries()
        {
            var _db = _client.GetDatabase("MiddleEgyptianDictionary");
            return _db.GetCollection<DictionaryEntry>("Entries");
        }

        public IMongoCollection<KeywordSearch> GetExistingKeywordsFromDb()
        {
            var _db = _client.GetDatabase("MiddleEgyptianDictionary");
            return _db.GetCollection<KeywordSearch>("Keywords");
        }

        public void DropEntryCollection()
        {
            var _db = _client.GetDatabase("MiddleEgyptianDictionary");
            _db.DropCollection("Entries");
        }

        public void DropKeywordCollection()
        {
            var _db = _client.GetDatabase("MiddleEgyptianDictionary");
            _db.DropCollection("Keywords");
        }
    }
}
