using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MiddleEgyptianDictionary.DictionaryParser
{
    public class DicksonParser : FileParser
    {
        private Queue<string> partialLine = new Queue<string>();

        public DicksonParser(string fileName,
                             Dictionary<string, DictionaryEntry> hashTracker) : 
            base(fileName, hashTracker) { }

        public override void ParseAll()
        {
            using (PdfReader reader = new PdfReader(fileName))
            using (PdfDocument pdf = new PdfDocument(reader))
            {
                for (int i = 1; i <= 617; i++)
                {
                    Debug.WriteLine("Page " + i.ToString());
                    ParsePageHelper(i, reader, pdf);
                }
            }
        }

        public void ParsePageHelper(int pageNumber, PdfReader reader, PdfDocument pdf)
        {
            String text = PdfTextExtractor.GetTextFromPage(pdf.GetPage(pageNumber), new LocationTextExtractionStrategy());

            List<string> lineList = CombineLineHelper(text);

            if (!lineList[lineList.Count - 1].EndsWith("}"))
            {
                partialLine.Enqueue(text.Substring(text.LastIndexOf('\n') + 1));
            }
            if (!lineList[0].StartsWith("["))
            {
                lineList[0] = partialLine.Dequeue() + " " + lineList[0];
            }

            foreach (string item in lineList)
            {
                ParseLineHelper(item);
            }
        }


        private List<string> CombineLineHelper(string text)
        {
            List<string> lineList = new List<string>();
            string currentLine = "";
            using (StringReader strReader = new StringReader(text))
            {
                string line;
                while ((line = strReader.ReadLine()) != null)
                {
                    if (line.StartsWith("[") && line.EndsWith("}"))
                    {
                        lineList.Add(line);
                    }
                    else if (line.StartsWith("["))
                    {
                        currentLine = line;
                    }
                    else if (line.EndsWith("}"))
                    {
                        currentLine += " " + line;
                        lineList.Add(currentLine);
                        currentLine = "";
                    }
                    else if ((line.Contains("-") || line.Contains("–")) && !line.Contains(")") && !line.Contains("{"))
                    {
                        continue;
                    }
                    else
                    {
                        currentLine += line;
                    }
                }
                if (currentLine != "")
                {
                    partialLine.Enqueue(currentLine);
                }
            }
            return lineList;
        }

        private void ParseLineHelper(string input)
        {
            int idxLBracket = input.IndexOf('[');
            int idxRBracket = input.IndexOf(']');
            int idxLParen = input.IndexOf('(');
            int idxRParen = input.IndexOf(')');
            int idxLCurly = input.IndexOf('{');
            int idxRCurly = input.IndexOf('}');

            string translitChunk = input.Substring(idxLBracket + 1, (idxRBracket - idxLBracket - 1));
            string translationChunk, posChunk;
            if (idxLParen != -1 && idxRParen != -1 && input.Contains("] ("))
            {
                posChunk = input.Substring(idxLParen + 1, (idxRParen - idxLParen - 1));
                translationChunk = input.Substring(idxRParen + 2, (idxLCurly - idxRParen - 2));


                if (String.IsNullOrWhiteSpace(translationChunk) && !String.IsNullOrWhiteSpace(posChunk))
                {
                    translationChunk = posChunk;
                    posChunk = "";
                }
            }
            else
            {
                translationChunk = input.Substring(idxRBracket + 2, (idxLCurly - idxRBracket - 2));
                posChunk = "";
            }
            string gardinerChunk = input.Substring(idxLCurly + 1, (idxRCurly - idxLCurly - 1));
            CreateEntry(translitChunk, translationChunk.Trim(), posChunk, FixDoubledGardinerIds(gardinerChunk), DataSource.dickson);
        }

        public static string FixDoubledGardinerIds(string gardinerChunk)
        {
            string answer = gardinerChunk;
            var matches = Regex.Matches(gardinerChunk, @"(([A-Za-z]|Aa)[0-9]+){2,}");
            for (int i = 0; i < matches.Count; i++)
            {
                var captures = matches[i].Groups[1].Captures;
                var reconstructed = String.Join(" ", captures.OfType<Capture>().Select(x => x.Value).ToArray());
                answer = answer.Replace(matches[i].Groups[0].Value, reconstructed);
            }
            return answer;
        }
    }
}

