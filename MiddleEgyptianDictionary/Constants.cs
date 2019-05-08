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
        public static string VygusDictionary2012PdfPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Resources", "MarkVygusDictionary.pdf");
        public static string VygusDictionary2018PdfPath = @"C:\Users\lfr2l\U of T\NML340\VYGUS_Dictionary_2018.pdf";
        public const int Vygus2018LastPage = 2568;
        public const int Vygus2012FirstPage = 24;
        public const int Vygus2012LastPage = 2267;
    }
}
