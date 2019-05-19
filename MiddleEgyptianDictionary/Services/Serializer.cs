using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MiddleEgyptianDictionary.Services
{
    static class Serializer
    {
        public static IEnumerable<BsonDocument> DataToBSon(IEnumerable<DictionaryEntry> data)
        {
            return from item in data
                   select item.ToBsonDocument();
        }

        public static Boolean SaveData(string path, object data)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                    formatter.Serialize(stream, data);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static object LoadData(string path)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    return formatter.Deserialize(stream);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
