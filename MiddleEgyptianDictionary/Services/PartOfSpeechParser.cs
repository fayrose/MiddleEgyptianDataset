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
            if (posString == null)
                return null;
            string[] splitPos = posString.Split()
                                          .Where(x => Regex.IsMatch(x, @"(\w)+"))
                                          .ToArray();
            
            for (int i = 0; i < splitPos.Count(); i++)
            {
                switch (splitPos[i])
                {
                    case "n.":
                    case "vnoun":
                        splitPos[i] = "noun";
                        break;
                    case "v.":
                        splitPos[i] = "verb";
                        break;
                    case "adj.":
                    case "djective":
                        splitPos[i] = "adjective";
                        break;
                    case "prn.":
                        splitPos[i] = "pronoun";
                        break;
                    case "part.":
                        splitPos[i] = "particle";
                        break;
                    case "int.":
                        splitPos[i] = "interrogative";
                        break;
                    case "trans.":
                        splitPos[i] = "transitive";
                        break;
                    case "intrans.":
                        splitPos[i] = "intransitive";
                        break;
                    case "fem.":
                    case "female":
                    case "femimine":
                        splitPos[i] = "feminine";
                        break;
                    case "masc.":
                    case "male":
                        splitPos[i] = "masculine";
                        break;
                    case "adv.":
                        splitPos[i] = "adverb";
                        break;
                    case "def.":
                        splitPos[i] = "definite";
                        break;
                    case "indef.":
                        splitPos[i] = "indefinite";
                        break;
                    case "art.":
                        splitPos[i] = "article";
                        break;
                    case "pos.":
                        splitPos[i] = "possessive";
                        break;
                    case "ind.":
                    case "indepentdent":
                        splitPos[i] = "independent";
                        break;
                    case "dependant":
                        splitPos[i] = "dependent";
                        break;
                    case "causitive":
                    case "ausative":
                        splitPos[i] = "causative";
                        break;
                    case "prep.":
                        splitPos[i] = "preposition";
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
