using MiddleEgyptianDictionary.DictionaryParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiddleEgyptianDictionary.Interfaces
{
    public interface IFactory<T>
    {
        T CreateInstance(Dictionary<string, DictionaryEntry> entryLocation);
    }
}
