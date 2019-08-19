using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MiddleEgyptianDictionary.Services
{
    public static class ManuelDeCodageToRESConverter
    {
        private static readonly Dictionary<String, String> translationDic = new Dictionary<string, string>
        {
            // Manuel to Codage needing translation
            { "D153", "D26" },
            { "O38A", "O38[mirror]" },
            { "R8A", "R8*[sep=0,fix]R8*[sep=0,fix]R8" },
            { "S56", "S7" },
            { "S14B", "S14b" },
            { "U6A", "U6a" },
            { "U6B", "U6b" },
            { "V30A", "V30a" },
            { "V40A", "V40a" },
            { "W3A", "W3a" },
            { "W14A", "W14a" },
            { "W24A", "W24a" },
            { "T26F", "T26" },
            { "A51&X1", "insert[b,sep=0.2](A51,X1)" },
            { "Y1V", "Y1a" },

            // VYGUS ERRORS - Replaces undisplayable Vygus' characters
            { "A33B", "A33" },
            { "A95", "A15" },
            { "A21A", "A21" },
            { "A34A", "A34" },
            { "A36C", "A37" },
            { "A4B", "A4" },
            { "A4C", "A4" },
            { "A43B", "A41" },
            { "A299B", "A41" },
            { "A40B", "A42" },
            { "A59B", "A59" },
            { "A133A", "A6" },
            { "N90", "D12" },
            { "Y24", "D12" },
            { "D26A", "D26" },
            { "D210", "D36" },
            { "D46D", "D46" },
            { "E102B", "E4" },
            { "F16A", "F16[mirror]" },
            { "F37D", "F37a" },
            { "F37E", "F37a" },
            { "F37AA", "F37a" },
            { "F37B", "F37a" },
            { "F37F", "F37a" },
            { "F37J", "F37a" },
            { "F39A", "F39" },
            { "F51D", "F51" },
            { "F51F", "F51" },
            { "G22A", "G22" },
            { "G29A", "G29" },
            { "G49E", "G49" },
            { "G237", "G49" },
            { "G167", "G50" },
            { "G7C", "G7" },
            { "H6B", "H6[rotate=330]" },
            { "I14A", "I14" },
            { "I14B", "I14" },
            { "I14C", "I14" },
            { "O202", "M13" },
            { "O353", "M13" },
            { "M4B", "M4" },
            { "M7A", "M7" },
            { "N11A", "N11[rotate=90]" },
            { "N62A", "N12[rotate=180]" },
            { "N21A", "N21[mirror]" },
            { "N24E", "N24" },
            { "S106", "N37" },
            { "N8A", "N8" },
            { "O29V", "O29[rotate=90]" },
            { "O30U", "O30[rotate=180]" },
            { "O40A", "O40" },
            { "O48A", "O48" },
            { "P4A", "P4" },
            { "P30", "P4" },
            { "P34", "P4" },
            { "P36", "P4" },
            { "Q12A", "Q2" },
            { "R1E", "R1" },
            { "R10B", "R10" },
            { "R15A", "R15" },
            { "R3C", "R3" },
            { "R3P", "R3" },
            { "S15A", "S15" },
            { "S116", "S27:S27" },
            { "G56", "stack(A,a)" },
            { "G57", "stack(A,f)" },
            { "V71", "stack(a,H)" },
            { "G58", "stack(Aa15,A)" },
            { "M145", "stack(Aa15,i)" },
            { "I34", "stack(b,D)" },
            { "D170A", "stack(D28,D52)" },
            { "D189", "stack(D32, W24)" },
            { "V41", "stack(D37,Z7)" },
            { "G225", "stack(G29,U7)" },
            { "V81", "stack(k,H)" },
            { "V90", "stack(k,V29)" },
            { "G87", "stack(m,D40)" },
            { "M159", "stack(M23, a)" },
            { "O91", "stack(O6, a)" },
            { "O90", "stack(O6, O29)" },
            { "R78", "stack(R15, a)" },
            { "T69", "stack(T14, a)" },
            { "T29C", "stack(T28, T30)" },
            { "U1A", "stack(U1,i)" },
            { "T14C", "T14[mirror]" },
            { "T19B", "T19" },
            { "T79", "T19" },
            { "T21V", "T21[rotate=270]" },
            { "T24E", "T24[mirror]" },
            { "T30A", "T30" },
            { "T30B", "T30A" },
            { "S123", "T32A" },
            { "S126", "T32A" },
            { "T9C", "T9" },
            { "U19A", "U19" },
            { "U32B", "U32" },
            { "U39Q", "U39" },
            { "U39L", "U40" },
            { "U7A", "U8" },
            { "S89", "V1" },
            { "V36G", "V36" },
            { "V5A", "V5[rotate=330]" },
            { "W15B", "W15" },
            { "W21A", "W21" },
            { "Y2V", "Y1A" },
            { "Y8A", "Y8" },
            { "Z11A", "Z11" },

            // NUMBERS - Converts numbers to RES
            { "Z15A", "Z1*Z1" },
            { "Z15B", "Z1*Z1*Z1" },
            { "Z15C", "Z1*Z1*Z1*Z1" },
            { "Z15I", ".*[sep=0]Z1*Z1*[sep=0].:Z1*Z1*Z1" },
            { "Z15", "Z1" },
            { "V20I", "V20*V20" },
            { "V20J", "V20*V20*V20" },
            { "V20K", "V20*V20*V20*V20" },
            { "V20L", ".*[sep=0]V20*V20*[sep=0].:V20*V20*V20" },
            { "100", "V1" },
            { "V1A", "V1*V1" },
            { "V1B", "V1*V1*V1" },
            { "V1C", "V1*V1*V1*V1" },
            { "V1I", ".*[sep = 0]V1*V1*[sep=0].:V1*V1*V1" },
            { "1000", "M12" },
            { "D50A", "D50*[fix,sep=0.2]D50" },
            { "D50B", "D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50" },
            { "D50C", "D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50" },
            { "D50D", "D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50:[fix,sep=0.2].*[sep=0]D50*[fix,sep=0.2]D50*[sep=0]." },
            { "D50E", "D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50:[fix,sep=0.2]D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50" },
            { "D50F", "D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50:[fix,sep=0.2,size=inf].*[sep=0.2] D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50*[sep=0.2]." },
            { "D50G", "D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50:[fix,sep=0.2]D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50" },
            { "D50H", "D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50:[fix,sep=0.2]D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50:[fix,sep=0.2]D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50" },
            { "D50I", "D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50*[fix,sep=0.2]D50" },
            { "D67A", "D67:[fix,sep=0.3]D67" },
            { "D67B", "D67*[fix,sep=0.3]D67:[fit,fix,sep=0.3]D67" },
            { "D67C", "D67*[fix,sep=0.3]D67:[fix,sep=0.3]D67*[fix,sep=0.3]D67" },
            { "D67D", "D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67:[fit,fix,sep=0.3]D67*[fix,sep=0.3]D67" },
            { "D67E", "D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67:[fix,sep=0.3]D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67" },
            { "D67F", "D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67:[fit,fix,sep=0.3,size=inf]D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67" },
            { "D67G", "D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67:[fix,sep=0.3]D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67" },
            { "D67H", "D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67:[fix,sep=0.3]D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67:[fix,sep=0.3]D67*[fix,sep=0.3]D67*[fix,sep=0.3]D67" },
            { "F51A", "F51*[fit,fix,sep=0.2]F51*[fit,fix,sep=0.2]F51" },
            { "F51B", "F51:[fit,fix,sep=0.2]F51:[fit,fix,sep=0.2]F51" },
            { "M12A", "M12:[fix,sep=0.3]M12" },
            { "M12B", "M12*[fix,sep=0.3]M12:[fit,fix,sep=0.3]M12" },
            { "M12C", "M12*[fix,sep=0.3]M12:[fix,sep=0.3]M12*[fix,sep=0.3]M12" },
            { "M12D", "M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12:[fit,fix,sep=0.3]M12*[fix,sep=0.3]M12" },
            { "M12E", "M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12:[fix,sep=0.3]M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12" },
            { "M12F", "M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12:[fit,fix,sep=0.3,size=inf]M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12" },
            { "M12G", "M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12:[fix,sep=0.3]M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12" },
            { "M12H", "M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12:[fix,sep=0.3]M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12:[fix,sep=0.3]M12*[fix,sep=0.3]M12*[fix,sep=0.3]M12" },
            { "N33A", "N33*[sep=0.5]N33*[sep=0.5] N33" },
            { "A14&Z2", "insert[bs,sep=0.5](A14,Z1*[sep=1.5]Z1*[sep=1.5]Z1)" },
            { "A17&Z2", "insert[be](A17*[sep=0.0]empty[width=0.2,height=0.0],Z1*Z1*Z1)" },
            { "A24&Z2D", "insert[te,sep=0.5](A24*[sep=0.0]empty[width=0.3],Z1*[sep=0.5]Z1*[sep=0.5]Z1)" },
            { "Z2A", "Z1*Z1*Z1" },
            { "Z2B", "D67*[sep=0.5]D67*[sep=0.5]D67" },
            { "Z2C", "Z1:[fix,sep=0.3]Z1*[sep=2.0]Z1" },
            { "Z2D", "Z1*[sep=2.0]Z1:[fix,sep=0.3]Z1" },
            { "Z2", "Z1*[sep=2.0]Z1*[sep=2.0]Z1" },
            { "Z3A", "Z1[rotate=90]:Z1[rotate=90]:Z1[rotate=90]" },
            { "Z3B", "D67:D67:D67" },
            { "Z3", "Z1:[sep=0.3]Z1:[sep=0.3]Z1" },
            { "Z4A", "Z1*[sep=2]Z1" },
            { "Z4B", "Z1[rotate=90]:Z1[rotate=90]" }
        };

        public static IEnumerable<string> ConvertMany(IEnumerable<string> MdCs)
        {
            return MdCs.Select(x => ConvertString(x));
        }

        public static string ConvertString(string MdC)
        {
            string answer = MdC;
            foreach (var key in translationDic.Keys)
                answer = answer.Replace(key, translationDic[key]);
            
            answer = answer.Replace('&', ':');
            return PostFixLetterToLower(answer);
        }
        
        private static string PostFixLetterToLower(string resInput)
        {
            string answer = resInput;
            var matches = Regex.Matches(resInput, @"(([A-Z]|Aa|AA)[0-9]+)([A-Za-z]+)");
            foreach (Match match in matches)
            {
                var replacement = match.Groups[1].Value + match.Groups[3].Value.ToLower();
                answer = Regex.Replace(answer, match.Value, replacement);
            }
            return answer;
        }
    }
}
