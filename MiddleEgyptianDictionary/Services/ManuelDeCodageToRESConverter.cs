using System;
using System.Collections.Generic;
using System.Linq;

namespace MiddleEgyptianDictionary.Services
{
    public static class ManuelDeCodageToRESConverter
    {
        public static IEnumerable<string> ConvertMany(IEnumerable<string> MdCs)
        {
            return MdCs.Select(x => ConvertString(x));
        }

        public static string ConvertString(string MdC)
        {
            string answer = MdC.Replace("D153", "D26");
            answer = MdC.Replace("M22A", "M22*M22");
            answer = MdC.Replace("O38A", "O38[mirror]");
            answer = MdC.Replace("R8A", "R8*[sep=0,fix]R8*[sep=0,fix]R8");
            answer = MdC.Replace("S56", "S7");
            return ConvertNumbers(answer);
        }

        private static string ConvertNumbers(string MdC)
        {
            string answer = MdC;
            Dictionary<String, String> dic = new Dictionary<string, string>();
            dic.Add("Z15", "Z1");
            dic.Add("Z15A", "Z1*Z1");
            dic.Add("Z15B", "Z1*Z1*Z1");
            dic.Add("Z15C", "Z1*Z1*Z1*Z1");
            dic.Add("Z15I", ".*[sep=0]Z1*Z1*[sep=0].:Z1*Z1*Z1");
            dic.Add("V20I", "V20*V20");
            dic.Add("V20J", "V20*V20*V20");
            dic.Add("V20K", "V20*V20*V20*V20");
            dic.Add("V20L", ".*[sep=0]V20*V20*[sep=0].:V20*V20*V20");
            dic.Add("100", "V1");
            dic.Add("V1A", "V1*V1");
            dic.Add("V1B", "V1*V1*V1");
            dic.Add("V1C", "V1*V1*V1*V1");
            dic.Add("V1I", ".*[sep = 0]V1*V1*[sep=0].:V1*V1*V1");
            dic.Add("1000", "M12");
            
            foreach (var key in dic.Keys)
                answer = answer.Replace(key, dic[key]);
            return answer;
        }
    }
}
