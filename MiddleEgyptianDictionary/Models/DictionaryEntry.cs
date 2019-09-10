using System.Collections.Generic;
using System.Linq;
using MiddleEgyptianDictionary.Services;
using MiddleEgyptianDictionary.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MiddleEgyptianDictionary
{
    public class DictionaryEntry
    {
        public DictionaryEntry()
        {
            this.Translations = new HashSet<Translation>();
        }
        [BsonId]
        public ObjectId Id { get; set; }
        public string Transliteration { get; set; }
        public string GardinerSigns { get; set; }
        public string ManuelDeCodage { get; set; }
        public string Res { get; set; }
        public virtual ICollection<Translation> Translations { get; set; }

        public override string ToString()
        {
            return GardinerConverter.ConvertGardiner(this.GardinerSigns) + 
                   "(" + this.Transliteration + "): " + 
                   this.Translations.FirstOrDefault().translation;  
        }
    }
}
