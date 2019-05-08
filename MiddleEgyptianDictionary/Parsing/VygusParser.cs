using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using MiddleEgyptianDictionary.Parsing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MiddleEgyptianDictionary.DictionaryParser
{
    public class VygusParser : FileParser
    {
        private readonly int startPage, endPage;
        private readonly bool is2018;
        public VygusParser(string pdfName,
                           Dictionary<string, DictionaryEntry> hashTracker,
                           bool is2018 = false) :
            base(pdfName, hashTracker)
        {
            startPage = is2018 ? 1 :Constants.Vygus2012FirstPage;
            endPage = is2018 ? Constants.Vygus2018LastPage : Constants.Vygus2012LastPage;
            this.is2018 = is2018;
        }

        public override void ParseAll()
        {
            using (PdfReader reader = new PdfReader(fileName))
            using (PdfDocument pdf = new PdfDocument(reader))
            {
                for (int i = startPage; i <= endPage; i++)
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
            if (is2018)
                return rawData;
            StringBuilder newText = new StringBuilder();
            List<string> wordList = rawData.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
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
            int[] pageList = is2018 ? new int[] { } : new int[] { 1560, 1658 };
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
                //Console.WriteLine(String.Format("Page {0}: word {1}", pageNumber, j));
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
            int idxSplit = wordData.IndexOf("   ");
            idxSplit = idxSplit == -1 ? idxSplit = wordData.IndexOf("  ") : idxSplit;

            // If the part of the speech is defined for this word
            if ((idxLeftBracket != -1) && (idxRightBracket != -1))
            {
                Debug.Assert((idxRightBracket + 1) < wordData.Length);

                translationChunk = wordData.Substring(0, idxLeftBracket);
                posChunk = wordData.Substring(idxLeftBracket, idxRightBracket - idxLeftBracket).Substring(1);
                if (idxRightCurly != -1 && idxRightCurly > idxRightBracket)
                {
                    signChunk = wordData.Substring(idxRightCurly + 1);
                    if (idxRightCurly != -1 && idxLeftCurly != -1)
                    {
                        translationChunk += " ";
                        translationChunk += wordData.Substring(idxLeftCurly, idxRightCurly - idxLeftCurly + 1);
                        translationChunk += " ";
                        if (rightParen > idxRightCurly)
                        {
                            translationChunk += wordData.Substring(leftParen, rightParen - leftParen + 1);
                            signChunk = wordData.Substring(rightParen + 1);
                        }
                    }
                }
                else if (rightParen != -1 && rightParen > idxLeftCurly)
                {
                    signChunk = idxRightBracket > rightParen ?
                                wordData.Substring(idxRightBracket + 1) :
                                wordData.Substring(rightParen + 1);
                    if (translationChunk.Last() != ' ')
                        translationChunk += " ";
                    if (idxLeftCurly != -1 && idxLeftCurly < rightParen)
                        translationChunk += wordData.Substring(idxLeftCurly, rightParen - idxLeftCurly + 1);
                    else if (leftParen != -1 && leftParen < rightParen)
                        translationChunk += wordData.Substring(leftParen, rightParen - leftParen + 1);
                    else if (idxLeftBracket != -1 && idxLeftBracket < rightParen)
                        translationChunk += wordData.Substring(idxLeftBracket, rightParen - idxLeftBracket + 1);
                }
                else
                {
                    if (idxRightBracket < idxLeftCurly && idxRightCurly == -1)
                    {
                        // Can't use max in case aposAfterLeftCurly = -1 and lastApos > -1
                        // as it would give an apostrophe within the translation, not after idxLeftCurly
                        int aposAfterLeftCurly = wordData.IndexOf('\'', idxLeftCurly + 1);
                        int lastApos = wordData.LastIndexOf('\'');
                        aposAfterLeftCurly = lastApos > aposAfterLeftCurly ? lastApos : aposAfterLeftCurly;

                        if (aposAfterLeftCurly != -1)
                        {
                            signChunk = wordData.Substring(aposAfterLeftCurly + 1);
                            translationChunk += " " + wordData.Substring(idxLeftCurly, aposAfterLeftCurly - idxLeftCurly + 1) + "}";
                        }
                        else
                            signChunk = wordData.Substring(idxLeftCurly + 1);
                    }
                    else if (idxSplit > idxRightBracket)
                    {
                        translationChunk = wordData.Substring(0, idxSplit);
                        signChunk = wordData.Substring(idxSplit + 1);
                    }
                    else
                    {
                        signChunk = wordData.Substring(idxRightBracket + 1);
                        signChunk = signChunk.Replace(" ? ", "");
                    }
                }
            }
            // If only the sign list and the translation are defined
            else
            {
                if (idxRightCurly != -1 )
                {
                    translationChunk = wordData.Substring(0, idxRightCurly + 1);
                    signChunk = wordData.Substring(idxRightCurly + 2);
                    int firstLeftCurly = wordData.IndexOf('{');
                    if (firstLeftCurly != idxLeftCurly && idxRightBracket != -1)
                    {
                        translationChunk = translationChunk.Remove(firstLeftCurly, idxRightBracket - firstLeftCurly + 1);
                        posChunk = wordData.Substring(firstLeftCurly + 1, idxRightBracket - firstLeftCurly - 1);
                    }
                }
                else
                {
                    if (idxSplit == -1)
                    {
                        signChunk = wordData;
                    }
                    else
                    {
                        translationChunk = wordData.Substring(0, idxSplit);
                        signChunk = wordData.Substring(idxSplit);

                    }
                }
                if (idxRightCurly == -1 && idxLeftCurly != -1 && idxRightBracket > idxLeftCurly)
                {
                    translationChunk = wordData.Substring(0, idxRightBracket + 1);
                    signChunk = wordData.Substring(idxRightBracket + 2);
                }
                else if (rightParen != -1 && rightParen > idxLeftCurly && rightParen > idxSplit)
                {
                    signChunk = wordData.Substring(rightParen + 1);
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
                else if (idxRightBracket != -1 && idxRightBracket > idxSplit && idxSplit != -1 && translationChunk.Length == 0)
                {
                    translationChunk = wordData.Substring(0, idxSplit);
                    posChunk = wordData.Substring(idxSplit, idxRightBracket - idxSplit);
                    signChunk = wordData.Substring(idxRightBracket + 1);
                }

            }
            // Remove extraneous whitespace in string
            signChunk = String.Join(" ", signChunk.Split(new string[] { " - ", "-", " "}, StringSplitOptions.RemoveEmptyEntries));
            string transliterationChunk = String.Join(" ", transliteration.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            posChunk = String.Join(" ", posChunk.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            translationChunk = String.Join(" ", translationChunk.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

            CreateEntry(transliterationChunk.Trim(),
                translationChunk.Trim(),
                posChunk.Trim(),
                signChunk.Trim(),
                DataSource.vygus);
        }
    }
}

