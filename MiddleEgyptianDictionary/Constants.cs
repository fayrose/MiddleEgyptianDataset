using System.IO;

namespace MiddleEgyptianDictionary.Parsing
{
    public class Constants
    {
        public static string DicksonDictionaryPdfPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Resources", "DicksonDictionary_.pdf");
        public static string LexiconDictionaryPdfPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Resources", "Lexicon.txt");
        public static string VygusDictionary2012PdfPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Resources", "MarkVygusDictionary.pdf");
        public static string VygusDictionary2018PdfPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Resources", "VYGUS_Dictionary_2018.pdf");
        public const int Vygus2018LastPage = 2568;
        public const int Vygus2012FirstPage = 24;
        public const int Vygus2012LastPage = 2267;
        public static string MDCTextsLocation = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "mdc_texts");
        public static string MDCTrigramsLocation = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "data_output", "Trigrams.txt");
        public static string gardinerToMDCLocation = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "data_output", "gardinerToMDC.txt");
        public static string gardinerListLocation = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "data_output", "gardinerSignList.txt");
    }
}
