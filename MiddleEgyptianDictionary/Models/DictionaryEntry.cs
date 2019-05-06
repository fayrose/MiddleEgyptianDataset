using System.Collections.Generic;
using System.Linq;
using MiddleEgyptianDictionary.Services;
using MiddleEgyptianDictionary.Models;

namespace MiddleEgyptianDictionary
{
    public class DictionaryEntry
    {
        public DictionaryEntry()
        {
            this.Translations = new HashSet<Translation>();
        }
        public string transliteration { get; set; }
        public string gardinerSigns { get; set; }
        public virtual ICollection<Translation> Translations { get; set; }

        public override string ToString()
        {
            return GardinerConverter.ConvertGardiner(this.gardinerSigns) + 
                   "(" + this.transliteration + "): " + 
                   this.Translations.FirstOrDefault().translation;  
        }


    }
}
