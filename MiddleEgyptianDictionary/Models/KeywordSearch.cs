using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MiddleEgyptianDictionary.Models
{
    public class KeywordSearch
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonSerializer(typeof(KeywordSearchSerializer))]
        public string Keyword { get; set; }

        public ICollection<ObjectId> EntryIds { get; set; }

        public KeywordSearch(string keyword, ObjectId entryId)
        {
            EntryIds = new HashSet<ObjectId>() { entryId };
            Id = ObjectId.GenerateNewId();
            Keyword = keyword;
        }

        public void AddIdToEntryIds(ObjectId id)
        {
            EntryIds.Add(id);
        }
    }
}
