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
            Dictionary<String, String> dic = new Dictionary<string, string>();
            dic.Add("D153", "D26");
            dic.Add("O38A", "O38[mirror]");
            dic.Add("R8A", "R8*[sep=0,fix]R8*[sep=0,fix]R8");
            dic.Add("S56", "S7");
            dic.Add("D27A", "D27[yscale=1.5]");
            dic.Add("D29", ".*D28:[sep=0.2,fit,fix]R12");
            dic.Add("D31A", "insert(empty[width=0.0,height=0.4]:D28,U36)");
            dic.Add("F13A", "F13[yscale=0.7]");
            dic.Add("G36A", "G36[yscale=0.8]");
            dic.Add("G37A", "G37[yscale=0.8]");
            dic.Add("M17A", "M17*[fix,sep=0.2]M17");
            dic.Add("M22A", "M22*[sep=0.5]M22");
            dic.Add("N19", "N18:[fix,sep=0.5]N18");
            dic.Add("N25A", "N25[yscale=0.8]");
            dic.Add("N35A", "N35:N35:N35");
            dic.Add("O30A", "O30*[sep=0.0]O30*[sep=0.0]O30*[sep=0.0]O30");
            dic.Add("S14B", "S40:[sep=0.2,fix]S12");
            dic.Add("T15", "T14[rotate=10]");
            dic.Add("U2", "U1[yscale=0.7]");
            dic.Add("U5", "U4[yscale=0.7]");
            dic.Add("U6", "U7[rotate=45]");
            dic.Add("U6A", "U7[rotate=75]");
            dic.Add("U6B", "U7[mirror,rotate=285]");
            dic.Add("V30A", "V30[yscale=0.8]");
            dic.Add("V40A", "V40*[fix,sep=0.1]V40");
            dic.Add("W3A", "W3[yscale=0.7]");
            dic.Add("W14A", "V28*W14:[sep=0.3,fit]O34");
            dic.Add("W24A", "W24*[sep=0.1]W24*[sep=0.1]W24");
            
            string answer = MdC;
            foreach (var key in dic.Keys)
                answer = answer.Replace(key, dic[key]);
            
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
            dic.Add("D50A", "D50*[fix,sep=0.2]D50");
            dic.Add("D50B", "D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50");
            dic.Add("D50C", "D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50");
            dic.Add("D50D", "D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50:[fix,sep=0.2].*[sep=0]D50*[fix,sep=0.2]D50*[sep=0].");
            dic.Add("D50E", "D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50:[fix,sep=0.2]D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50");
            dic.Add("D50F", "D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50:[fix,sep=0.2,size=inf].*[sep=0.2] D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50*[sep=0.2].");
            dic.Add("D50G", "D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50:[fix,sep=0.2]D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50");
            dic.Add("D50H", "D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50:[fix,sep=0.2]D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50:[fix,sep=0.2]D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50");
            dic.Add("D50I", "D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50");
            dic.Add("D67A", "D67:[fix,sep=0.3]D67");
            dic.Add("D67B", "D67*[fix,sep=0.3]D67:[fit,fix,sep=0.3]D67");
            dic.Add("D67C", "D67*[fix,sep=0.3]D67:[fix,sep=0.3]D67*[fix,sep=0.3]D67");
            dic.Add("D67D", "D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67:[fit,fix,sep=0.3]D67*[fix,sep=0.3]D67");
            dic.Add("D67E", "D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67:[fix,sep=0.3]D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67");
            dic.Add("D67F", "D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67:[fit,fix,sep=0.3,size=inf]D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67");
            dic.Add("D67G", "D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67:[fix,sep=0.3]D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67");
            dic.Add("D67H", "D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67:[fix,sep=0.3]D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67:[fix,sep=0.3]D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67");
            dic.Add("F51A", "F51*[fit,fix,sep=0.2]F51*[fit,fix,sep=0.2]F51");
            dic.Add("F51B", "F51:[fit,fix,sep=0.2]F51:[fit,fix,sep=0.2]F51");
            dic.Add("M12A", "M12:[fix,sep=0.3]M12");
            dic.Add("M12B", "M12*[fix,sep=0.3]M12:[fit,fix,sep=0.3]M12");
            dic.Add("M12C", "M12*[fix,sep=0.3]M12:[fix,sep=0.3]M12*[fix,sep=0.3]M12");
            dic.Add("M12D", "M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12:[fit,fix,sep=0.3]M12*[fix,sep=0.3]M12");
            dic.Add("M12E", "M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12:[fix,sep=0.3]M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12");
            dic.Add("M12F", "M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12:[fit,fix,sep=0.3,size=inf]M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12");
            dic.Add("M12G", "M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12:[fix,sep=0.3]M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12");
            dic.Add("M12H", "M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12:[fix,sep=0.3]M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12:[fix,sep=0.3]M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12");
            dic.Add("N33A", "N33*[sep=0.5]N33*[sep=0.5] N33");
            dic.Add("Z2", "Z1*[sep=2.0]Z1*[sep=2.0]Z1");
            dic.Add("Z2A", "Z1*Z1*Z1");
            dic.Add("Z2B", "D67*[sep=0.5]D67*[sep=0.5]D67");
            dic.Add("Z2C", "Z1:[fix,sep=0.3]Z1*[sep=2.0]Z1");
            dic.Add("Z2D", "Z1*[sep=2.0]Z1:[fix,sep=0.3]Z1");
            dic.Add("Z3", "Z1:[sep=0.3]Z1:[sep=0.3]Z1");
            dic.Add("Z3A", "Z1[rotate=90]:Z1[rotate=90]:Z1[rotate=90]");
            dic.Add("Z3B", "D67:D67:D67");
            dic.Add("Z4A", "Z1*[sep=2]Z1");
            foreach (var key in dic.Keys)
                answer = answer.Replace(key, dic[key]);
            return answer;
        }
    }
}
