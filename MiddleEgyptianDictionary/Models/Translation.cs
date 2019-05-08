namespace MiddleEgyptianDictionary.Models
{
    public class Translation
    {
        public string translation { get; set; }
        public virtual Metadata TranslationMetadata { get; set; }
    }
}
