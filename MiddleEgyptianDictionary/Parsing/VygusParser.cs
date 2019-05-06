using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MiddleEgyptianDictionary.DictionaryParser
{
    public class VygusParser : FileParser
    {
        public VygusParser(string pdfName, 
                           Dictionary<string, DictionaryEntry> hashTracker) : 
            base(pdfName, hashTracker) { }

        public override void ParseAll()
        {
            using (PdfReader reader = new PdfReader(fileName))
            using (PdfDocument pdf = new PdfDocument(reader))
            {
                for (int i = 24; i <= 2267; i++)
                {
                    Debug.WriteLine("Page " + i.ToString());
                    ParsePageHelper(i, reader, pdf);
                }
            }
        }

        private void ParsePageHelper(int pageNumber, PdfReader reader, PdfDocument doc)
        {
            String text = PdfTextExtractor.GetTextFromPage(doc.GetPage(pageNumber), new LocationTextExtractionStrategy());
            VygusAssociateDefinitions(VygusFilterText(text), pageNumber);
        }

        public void VygusParsePage(int pageNumber)
        {
            using (PdfReader reader = new PdfReader(fileName))
            using (PdfDocument pdf = new PdfDocument(reader))
            {
                String text = PdfTextExtractor.GetTextFromPage(pdf.GetPage(pageNumber), new LocationTextExtractionStrategy());
                VygusAssociateDefinitions(VygusFilterText(text), pageNumber);
            }
        }

        public string VygusFilterText(string rawData)
        {
            StringBuilder newText = new StringBuilder();
            List<string> wordList = rawData.Split(' ').ToList();
            for (int i = 0; i < wordList.Count; i++)
            {
                if (wordList[i].Contains("vygus"))
                {
                    wordList[i] = "\n";
                }
            }
            string reconstructed = String.Join(" ", wordList);
            return reconstructed;
        }

        public List<DictionaryEntry> VygusAssociateDefinitions(string reconstructed, int pageNumber)
        {
            List<string> wordList = reconstructed.Split('\n').ToList();
            List<DictionaryEntry> dictList = new List<DictionaryEntry>();
            int[] pageList = { 1560, 1658 };
            for (int i = 0; i < wordList.Count; i++)
            {
                if (String.IsNullOrWhiteSpace(wordList[i]))
                {
                    wordList.RemoveAt(i);
                    i--;
                }
            }
            if (!pageList.Contains(pageNumber))
            {
                wordList.RemoveAt(wordList.Count - 1);
            }

            if (wordList.Count % 2 == 1)
            {
                for (int k = 0; k < (wordList.Count - 1); k++)
                {
                    if (wordList[k].Contains('[') && wordList[k+1].Contains('['))
                    {
                        wordList.Insert((k + 1), "?");
                        break;
                    }
                    bool long_def = wordList[k].Contains('-') && wordList[k + 1].Contains('-');
                    long_def |= (wordList[k].Contains("(") && wordList[k+1].Contains(')'));
                    if (long_def)
                    {
                        wordList[k] = wordList[k] + wordList[k+1];
                        wordList.RemoveAt(k+1);
                        break;
                    }
                }
            }
            Debug.Assert(wordList.Count % 2 == 0);
            int halfCount = wordList.Count / 2;
            for (int j = 0; j < halfCount; j++)
            {
                ParseWordData(wordList[j * 2].Trim(), wordList[j * 2 + 1].Trim());
                Console.WriteLine(String.Format("Page {0}: word {1}", pageNumber, j));
            }

            return dictList;
        }

        private void ParseWordData(string wordData, string transliteration)
        {
            string translationChunk = "", signChunk = "", posChunk = "";
            int idxLeftBracket = wordData.IndexOf('[');
            int idxRightBracket = wordData.LastIndexOf(']');
            int idxLeftCurly = wordData.LastIndexOf('{');
            int idxRightCurly = wordData.LastIndexOf('}');
            int leftParen = wordData.IndexOf("(");
            int rightParen = wordData.LastIndexOf(")");

            // If the part of the speech is defined for this word
            if ((idxLeftBracket != -1) && (idxRightBracket != -1))
            {
                Debug.Assert((idxRightBracket + 1) < wordData.Length);

                translationChunk = wordData.Substring(0, idxLeftBracket);
                posChunk = wordData.Substring(idxLeftBracket, (idxRightBracket - idxLeftBracket)).Substring(1).Trim();
                if (idxRightCurly != -1 && idxRightCurly > idxRightBracket)
                {
                    signChunk = wordData.Substring(idxRightCurly + 1).Trim();
                }
                else if (rightParen != -1 && rightParen > idxLeftCurly)
                {
                    signChunk = idxRightBracket > rightParen ?
                                wordData.Substring(idxRightBracket + 1).Trim() :
                                wordData.Substring(rightParen + 1).Trim();
                    if (translationChunk.Last() != ' ')
                    {
                        translationChunk += " ";
                    }
                    if (idxLeftCurly != -1 && idxLeftCurly < rightParen)
                    {
                        translationChunk += wordData.Substring(idxLeftCurly, rightParen - idxLeftCurly + 1);
                    }
                    else if (leftParen != -1 && leftParen < rightParen)
                    {
                        translationChunk += wordData.Substring(leftParen, rightParen - leftParen + 1);
                    }
                    else if (idxLeftBracket != -1 && idxLeftBracket < rightParen)
                    {
                        translationChunk += wordData.Substring(idxLeftBracket, rightParen - idxLeftBracket + 1);
                    }                  
                }
                else
                {
                    if (idxRightBracket < idxLeftCurly && idxRightCurly == -1)
                    {
                        signChunk = wordData.Substring(idxLeftCurly + 1).Trim();
                    }
                    else
                    {
                        signChunk = wordData.Substring(idxRightBracket + 1).Trim();
                    }
                }
            }

            // If only the sign list and the translation are defined
            else
            {
                int idxSplit = wordData.IndexOf("   ");
                if (idxRightCurly != -1 )
                {
                    translationChunk = wordData.Substring(0, idxRightCurly + 1);
                    signChunk = wordData.Substring(idxRightCurly + 2);
                }
                else
                {
                    if (idxSplit == -1)
                    {
                        idxSplit = wordData.IndexOf("  ");
                        signChunk = wordData.Trim();
                    }
                    else
                    {
                        translationChunk = wordData.Substring(0, idxSplit);
                        signChunk = wordData.Substring(idxSplit).Trim();

                    }
                }
                if (idxRightCurly == -1 && idxLeftCurly != -1 && idxRightBracket > idxLeftCurly)
                {
                    translationChunk = wordData.Substring(0, idxRightBracket + 1);
                    signChunk = wordData.Substring(idxRightBracket + 2);
                }
                else if (rightParen != -1 && rightParen > idxLeftCurly && rightParen > idxSplit)
                {
                    signChunk = wordData.Substring(rightParen + 1).Trim();
                    if (translationChunk.LastOrDefault() != ' ')
                    {
                        translationChunk += " ";
                    }
                    if (idxLeftCurly != -1)
                    {
                        translationChunk += wordData.Substring(idxLeftCurly, rightParen - idxLeftCurly + 1);
                    }
                    else if (leftParen != -1)
                    {
                            
                        translationChunk += wordData.Substring(leftParen, rightParen - leftParen + 1);
                    }
                  
                }
                else if (idxRightBracket != -1 && idxRightBracket > idxSplit)
                {
                    translationChunk = wordData.Substring(0, idxSplit);
                    posChunk = wordData.Substring(idxSplit, idxRightBracket - idxSplit).Trim() ;
                    signChunk = wordData.Substring(idxRightBracket + 1).Trim();
                }

            }
            translationChunk = translationChunk.Trim();
            signChunk = String.Join(" ", signChunk.Split(new string[] { " - " }, StringSplitOptions.None));
            string transliterationChunk = String.Join(" ", transliteration.Trim().Split(' '));
            CreateEntry(transliterationChunk, translationChunk, posChunk, signChunk, DataSource.vygus);
        }
    }
}

