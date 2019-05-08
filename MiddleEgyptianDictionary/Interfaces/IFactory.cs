using System.Collections.Generic;

namespace MiddleEgyptianDictionary.Interfaces
{
    public interface IFactory<T>
    {
        T CreateInstance(Dictionary<string, DictionaryEntry> entryLocation);
    }
}
