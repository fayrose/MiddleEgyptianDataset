using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace MiddleEgyptianDictionary.DictionaryParser
{
    public class LexiconParser : FileParser
    {
        public LexiconParser(string fileName,
                             Dictionary<string, DictionaryEntry> hashTracker):
            base(fileName, hashTracker) { }

        public override void ParseAll()
        {
            StreamReader reader = File.OpenText(fileName);
            string line; 
            while ((line = reader.ReadLine()) != null)
            {
                ParseLine(line);
            }
            reader.Close();
        }
        
        private void ParseLine(string line)
        {
            string[] lineArr = line.Split(';');
            string signChunk = String.Join(" ", lineArr[0].Split(',')).Trim();
            Regex r = new Regex(@"\s+");
            string translitChunk = r.Replace(lineArr[1].Trim(), @" ");
            translitChunk = translitChunk.Replace("=", ".");
            string translationChunk = lineArr[2].Trim();
            CreateEntry(translitChunk, translationChunk, null, signChunk, DataSource.lexicon);
        }
        
    }
}
