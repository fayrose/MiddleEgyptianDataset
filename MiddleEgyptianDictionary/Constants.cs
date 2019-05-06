using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiddleEgyptianDictionary.Parsing
{
    public class Constants
    {
        public static string DicksonDictionaryPdfPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Resources", "DicksonDictionary_.pdf");
        public static string LexiconDictionaryPdfPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Resources", "Lexicon.txt");
        public static string VygusDictionaryPdfPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Resources", "MarkVygusDictionary.pdf");
    }
}
