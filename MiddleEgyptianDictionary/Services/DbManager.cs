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
            settings.SslSettings =
              new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            _client = new MongoClient(settings);
        }

        public async Task WriteEntriesToDbAsync(IEnumerable<DictionaryEntry> entries)
        {
            var _db = _client.GetDatabase("MiddleEgyptianDictionary");
            _db.DropCollection("Entries");
            var entryCollection = _db.GetCollection<DictionaryEntry>("Entries");
            await entryCollection.InsertManyAsync(entries);
        }

        public async Task WriteKeywordsToDbAsync(IEnumerable<KeywordSearch> keywords)
        {
            var _db = _client.GetDatabase("MiddleEgyptianDictionary");
            _db.DropCollection("Keywords");
            var entryCollection = _db.GetCollection<KeywordSearch>("Keywords");
            await entryCollection.InsertManyAsync(keywords);
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

        public void DropKeywordCollection()
        {
            var _db = _client.GetDatabase("MiddleEgyptianDictionary");
            _db.DropCollection("Keywords");
        }
    }
}
