using System;

namespace MiddleEgyptianDictionary.Models
{
    public class Metadata
    {
        public string PartOfSpeech { get; set; }
        public DataSource DictionaryName { get; set; }
        public int? Page { get; set; }
        public int? IndexOnPage { get; set; }

        internal object Select(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }
}
