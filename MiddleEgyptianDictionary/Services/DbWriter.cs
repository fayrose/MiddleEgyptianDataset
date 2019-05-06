using MiddleEgyptianDictionary.Parsing;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace MiddleEgyptianDictionary.Services
{
    static class DbWriter
    {
        public async static Task WriteToDbAsync(MiddleEgyptianDictionary med)
        {
            
            MongoClientSettings settings = MongoClientSettings.FromUrl(
              new MongoUrl(Constants.ConnectionString)
            );
            settings.SslSettings =
              new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var _client = new MongoClient(settings);
            var _db = _client.GetDatabase("MiddleEgyptianDDbictionary");

            var entryCollection = _db.GetCollection<BsonDocument>("Entries");
            await entryCollection.InsertManyAsync(med.SerializeDictionaries());
        }
    }
}
