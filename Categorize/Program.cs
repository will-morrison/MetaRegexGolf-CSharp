using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Categorize
{
    public class Program
    {
        /// <summary>
        /// 1. Given a list of categories, and an input word or corpus, 
        /// categorized that input as one of the categories
        /// 
        /// Do this by automatically getting a corpus of words for each category,
        /// from wikipedia, then using that to determine if the word/words in the input match.
        /// 
        /// 1st try will be Use Meta Regex creator to create a regex which will match 
        /// words from wiki article 1 but not from the other categories.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            const string StarTrek = "Star Trek";
            const string StarWars = "Star Wars";

            var trek = categoryToCorpus[StarTrek].Split('\n').ToList();
            trek = trek.Select(t => t.Replace("\r", String.Empty)).ToList();
            var wars = categoryToCorpus[StarWars].Split('\n').ToList();
            wars = wars.Select(t => t.Replace("\r", String.Empty)).ToList();

            GetRegex(StarTrek, StarWars, trek, wars, "as written");

            trek.Reverse();
            wars.Reverse();
            GetRegex(StarTrek, StarWars, trek, wars, "reversed lists");

            trek.Sort();
            wars.Sort();
            GetRegex(StarTrek, StarWars, trek, wars, "sorted");

            trek.Reverse();
            wars.Reverse();
            GetRegex(StarTrek, StarWars, trek, wars,"sorted then reversed");
            
            Console.WriteLine("Pause");
        }

        private static void GetRegex(string titleList1, string titleList2, List<string> list1, List<string> list2, string comment)
        {
            var reg = Categorize.Reg.MetaRegex.findRegex(list1, list2);
            Console.WriteLine("To Match \"{0}\" entries, but not \"{1}\" use regex:", titleList1, titleList2);
            Console.WriteLine("{0}  ({1} characters) {2}\n", reg, reg.Length, comment);
        }

        public static Dictionary<string, string> categoryToCorpus = new Dictionary<string, string> { 
            {"Star Wars", 
@"The Phantom Menace
Attack of the Clones
Revenge of the Sith
A New Hope
The Empire Strikes Back
Return of the Jedi"},
            {"Star Trek", 
@"The Wrath of Khan
The Search for Spock
The Voyage Home
The Final Frontier
The Undiscovered Country
Generations
First Contact
Insurrection
Nemesis"}
        };

        


        class TwoLists
        {
            public List<string> First;
            public List<string> Second;
            public string Name;

            public TwoLists(List<string> first, List<string> second, string name)
            {
                First = first;
                Second = second;
                Name = name;
            }
        }

        /// <summary>
        /// Find a regex to match A but not B, and vice-versa.  Print summary.
        ///     for (W, L, legend) in [(A, B, 'A-B'), (B, A, 'B-A')]:
        ///         solution = findregex(W, L)
        ///         assert verify(solution, W, L)
        ///         ratio = len('^(' + OR(W) + ')$') / float(len(solution))
        ///         print '%3d chars, %4.1f ratio, %2d winners %s: %r' % (
        ///             len(solution), ratio , len(W), legend, solution)
        /// </summary>
        private static string findboth(List<string> A, List<string> B)
        {
            var combinations = new List<TwoLists> { 
                new TwoLists(A, B, "A-B"),
                new TwoLists(B, A, "B-A")                            
            };

            foreach (var combo in combinations)
            {
                var solution = Categorize.Reg.MetaRegex.findRegex(combo.First, combo.Second);
                Debug.Assert(Categorize.Reg.MetaRegex.VerifyMatchesAllWinnersNoLosers(solution, combo.First, combo.Second));
                var ratio = ("^(" + Categorize.Reg.MetaRegex.OR(combo.First) + ")$").Length / solution.Length;

                Console.WriteLine("{0} chars, {1} ratio, {2} winners {3}: '{4}'",
                    solution.Length, ratio, combo.First.Count, combo.Name, solution);
            }

            return string.Empty;
        }

        public const string wikipedia = "https://en.wikipedia.org/wiki/";
        public static string wikiURL(string subject)
        {
            return wikipedia + System.Web.HttpUtility.UrlEncode(subject);
        }

        public static List<string> CleanAndSplitMultiLine(string input)
        {
            return input.Replace("\r\n", " ")
                .Replace("\n", " ")
                .Replace("  ", " ")
                .Replace("  ", " ")
                .Replace("  ", " ")
                .Split(' ')
                .Where(li => !String.IsNullOrEmpty(li))
                .Select(li => li.Trim())
                .ToList();
        }

        public static bool StringListsEqual(List<string> first, List<string> second)
        {
            bool allStringsEqual = true;
            if (first.Count != second.Count)
            { return false; }

            first.Sort();
            second.Sort();
            for (int i = 0; i < first.Count; i++)
            {
                allStringsEqual = (first[i] == second[i]) && allStringsEqual;
            }
            return allStringsEqual;
        }
    }
}
