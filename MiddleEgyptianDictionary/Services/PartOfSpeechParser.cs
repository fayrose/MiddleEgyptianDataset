using MiddleEgyptianDictionary.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MiddleEgyptianDictionary.Services
{
    public static class PartOfSpeechParser
    {
        public static HashSet<string> posSet = new HashSet<string>();
        public static string FixPartOfSpeech(string posString)
        {
            return ReplaceAcronymsWithFullWord(posString);
        }

        private static string ReplaceAcronymsWithFullWord(string partOfSpeech)
        {
            if (partOfSpeech == null)
                return null;
            string[] splitPos = partOfSpeech.Split()
                                          .Where(x => Regex.IsMatch(x, @"(\w)+"))
                                          .ToArray();

            for (int i = 0; i < splitPos.Count(); i++)
            {
                switch (splitPos[i])
                {
                    // PARTS OF SPEECH
                    case "n.":
                    case "vnoun":
                        splitPos[i] = Constants.Noun;
                        break;
                    case "v.":
                        splitPos[i] = Constants.Verb;
                        break;
                    case "adj.":
                    case "djective":
                        splitPos[i] = Constants.Adjective;
                        break;
                    case "prn.":
                        splitPos[i] = Constants.Pronoun;
                        break;
                    case "part.":
                        splitPos[i] = Constants.Particle;
                        break;
                    case "int.":
                        splitPos[i] = Constants.Interrogative;
                        break;
                    case "trans.":
                        splitPos[i] = Constants.Transitive;
                        break;
                    case "intrans.":
                        splitPos[i] = Constants.Intransitive;
                        break;
                    case "fem.":
                    case "female":
                    case "femimine":
                        splitPos[i] = Constants.Feminine;
                        break;
                    case "masc.":
                    case "male":
                        splitPos[i] = Constants.Masculine;
                        break;
                    case "adv.":
                        splitPos[i] = Constants.Adverb;
                        break;
                    case "def.":
                        splitPos[i] = Constants.Definite;
                        break;
                    case "indef.":
                        splitPos[i] = Constants.Indefinite;
                        break;
                    case "art.":
                        splitPos[i] = Constants.Article;
                        break;
                    case "pos.":
                        splitPos[i] = Constants.Possessive;
                        break;
                    case "ind.":
                    case "indepentdent":
                        splitPos[i] = Constants.Independent;
                        break;
                    case "dependant":
                        splitPos[i] = Constants.Dependent;
                        break;
                    case "causitive":
                    case "ausative":
                        splitPos[i] = Constants.Causitive;
                        break;
                    case "prep.":
                        splitPos[i] = Constants.Preposition;
                        break;

                    // SEMANTIC
                    case "ani.":
                    case "ani":
                        splitPos[i] = "animal";
                        break;
                    case "min.":
                    case "min":
                        splitPos[i] = "mineral";
                        break;
                    case "arch.":
                    case "arch":
                        splitPos[i] = "architecture";
                        break;
                    case "div.":
                    case "div":
                        splitPos[i] = "divinity";
                        break;
                    case "loc.":
                    case "loc":
                        splitPos[i] = "location";
                        break;
                    case "bod.":
                    case "bod":
                        splitPos[i] = "body";
                        break;
                    case "med.":
                        splitPos[i] = "medical";
                        break;
                    case "astro.":
                    case "astro":
                        splitPos[i] = "astronomy";
                        break;
                    case "furn.":
                    case "furn":
                        splitPos[i] = "furniture";
                        break;
                    default: break;
                }
                splitPos[i] = splitPos[i].Trim(new char[] { '.', '-', '[', ']', ',', '}', '{' });
                posSet.Add(splitPos[i]);
            }
            return string.Join(" ", splitPos);
        }
    }
}
