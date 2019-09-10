using System.IO;

namespace MiddleEgyptianDictionary
{
    public class Constants
    {
        // Resource Locations
        public static string DicksonDictionaryPdfPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Resources", "DicksonDictionary_.pdf");
        public static string LexiconDictionaryPdfPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Resources", "Lexicon.txt");
        public static string VygusDictionary2012PdfPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Resources", "MarkVygusDictionary.pdf");
        public static string VygusDictionary2018PdfPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Resources", "VYGUS_Dictionary_2018.pdf");
        public static string StopWordsLocation = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "Resources", "StopWords");
        public static string RepositoryLocation = Path.GetDirectoryName(Path.GetDirectoryName(
                                                  Path.GetDirectoryName(
                                                        System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase))).Substring(6);
        public static string ConnectionString = @"mongodb://localhost:27017";

        // Parsing Constants
        public const int Vygus2018LastPage = 2568;
        public const int Vygus2012FirstPage = 24;
        public const int Vygus2012LastPage = 2267;

        // Cached document information
        public static string MDCTextsLocation = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "mdc_texts");
        public static string MDCTrigramsLocation = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "data_output", "Trigrams.txt");
        public static string gardinerToMDCLocation = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "data_output", "gardinerToMDC.txt");
        public static string gardinerListLocation = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "data_output", "gardinerSignList.txt");
        

        // PART OF SPEECH CONSTANTS
        public const string Noun = "noun";
        public const string Verb = "verb";
        public const string Adjective = "adjective";
        public const string Pronoun = "pronoun";
        public const string Particle = "particle";
        public const string Interrogative = "interrogative";
        public const string Transitive = "transitive";
        public const string Intransitive = "intransitive";
        public const string Feminine = "feminine";
        public const string Masculine = "masculine";
        public const string Adverb = "adverb";
        public const string Definite = "definite";
        public const string Indefinite = "indefinite";
        public const string Article = "article";
        public const string Possessive = "possessive";
        public const string Independent = "independent";
        public const string Dependent = "dependent";
        public const string Causitive = "causative";
        public const string Preposition = "preposition";
    }
}
