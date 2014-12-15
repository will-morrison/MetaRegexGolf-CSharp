using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Categorize.Reg
{
    /// <summary>
    /// Ported from Peter Norvig's Python script at
    /// http://nbviewer.ipython.org/url/norvig.com/ipython/xkcd1313.ipynb?create=1
    /// originally concieved of by Randal from XKCD.com
    /// </summary>
    public static class MetaRegex
    {
        public static RegexOptions RegexCaseSensativity = RegexOptions.IgnoreCase;

        /// <summary>
        /// Find a regex that matches all winners but no losers (sets of strings)." <para />
        /// Make a pool of regex components, then pick from them to cover winners. <para />
        /// On each iteration, add the 'best' component to 'solution', <para />
        /// remove winners covered by best, and keep in 'pool' only components <para />
        /// that still match some winner. <para />
        ///  <para />
        /// pool = regex_components(winners, losers) <para />
        /// solution = []         <para />
        /// while winners: <para />
        ///    best = max(pool, key=score) <para />
        ///    solution.append(best) <para />
        ///    winners = winners - matches(best, winners) <para />
        ///    pool = {r for r in pool if matches(r, winners)} <para />
        /// return OR(solution) <para />
        /// </summary>
        public static string findRegex(List<string> winners, List<string> losers, bool caseSensative = false)
        {
            List<string> winnerCorpus = winners.ToArray().ToList(); //silly cloning
            List<string> loserCorpus = losers.ToArray().ToList(); //silly cloning
            if (caseSensative)
            {
                RegexCaseSensativity = RegexOptions.None;
            }

            List<string> pool = matchAtLeastOneWinnerButNoLosers(winnerCorpus, loserCorpus).ToList();
            List<string> winnerBestSolutions = new List<string>();
            string bestP = string.Empty;
            double bestScore = -1000;

            while (winnerCorpus.Any())
            {
                bestP = winnerCorpus.First();
                bestScore = score(bestP, winnerCorpus);

                foreach (var p in pool)
                {
                    var pScore = score(p, winnerCorpus);
                    if (pScore > bestScore)
                    {
                        bestP = p;
                        bestScore = pScore;
                    }
                }
                winnerBestSolutions.Add(bestP);
                // Move the best matches to the BestSolutionsList, and remove from winnerCorpus
                winnerCorpus.RemoveAll(w => matches(bestP, winnerCorpus).Contains(w));

                // Now only keep the pool ones that match the New set of winners.
                pool = pool.Where(p => matches(p, winnerCorpus).Any()).ToList();
            }


            return OR(winnerBestSolutions);
        }


        

        /// <summary>
        /// Return true iff the regex matches all winners but no losers.
        /// missed_winners = {W for W in winners if not re.search(regex, W)}
        ///     matched_losers = {L for L in losers if re.search(regex, L)}
        /// if missed_winners:
        ///     print "Error: should match but did not:", ', '.join(missed_winners)
        /// if matched_losers:
        ///     print "Error: should not match but did:", ', '.join(matched_losers)
        /// return not (missed_winners or matched_losers)
        /// </summary>
        public static bool VerifyMatchesAllWinnersNoLosers(string regex, List<string> winners, List<string> losers)
        {
            var missedWinners = winners.Where(w => !Regex.IsMatch(w, regex, RegexCaseSensativity));
            var matchedLosers = losers.Where(w => Regex.IsMatch(w, regex, RegexCaseSensativity));
            string message = string.Empty;
            string list = string.Empty;

            bool missed_winners = missedWinners.Any();
            bool matched_losers = matchedLosers.Any();
            if (missed_winners)
            {
                message += "Error: should match but did not: {0} - Count(" + missedWinners.Count() + ")\n";
                list += String.Join(", ", missedWinners);
            }
            if (matched_losers)
            {
                message += "Error: should not match but did: {0} - Count(" + matchedLosers.Count() + ")\n";
                list += String.Join(", ", matchedLosers);
            }
            if (missed_winners || matched_losers)
            {
                Console.WriteLine(message, list);
                Debug.WriteLine(String.Format(message, list));
            }
            return !(missed_winners || matched_losers);
        }

        /// <summary>
        /// # Join a sequence of strings with '|' between them
        /// </summary>
        public static string OR(List<string> inputs)
        {
            return String.Join("|", inputs);
        }

        const int magicScoreNumber = 4;
        /// <summary>
        /// def score(r): return 4 * len(matches(r, winners)) - len(r)
        /// Higher Score means better.  (more matches per length)
        /// </summary>
        public static int score(string regex, List<string> winners)
        {
            int numMatches = matches(regex, winners).Count;
            return (magicScoreNumber * numMatches) - regex.Length;
        }


        /// <summary>
        /// Return a set of all the strings that are matched by regex.
        /// </summary>
        public static List<string> matches(string regexPattern, List<string> inputStrings)
        {
            var regex = new Regex(regexPattern, RegexCaseSensativity);
            var anyMatchingStrings = inputStrings.Where(s => regex.IsMatch(s));
            if (anyMatchingStrings.Any())
            {
                return anyMatchingStrings.ToList();
            }
            return new List<string>();
        }



        /// <summary>
        /// Return components that match at least one winner, but no loser.
        /// wholes = {'^'+winner+'$' for winner in winners}
        /// parts = {d for w in wholes for s in subparts(w) for d in dotify(s)}
        /// return wholes | {p for p in parts if not matches(p, losers)}
        /// </summary>
        public static IEnumerable<string> matchAtLeastOneWinnerButNoLosers(List<string> winners, List<string> losers)
        {
            List<string> wholes = winners.Select(winner => String.Format("^{0}$", winner)).ToList();
            List<string> parts = new List<string>();
            foreach (var w in wholes)
            {
                foreach (var s in subparts(w))
                {
                    foreach (var d in dotify(s))
                    {
                        parts.Add(d);
                    }
                }
            }
            //TODO, not sure what to do with the Pipe here...
            IEnumerable<string> anyNonLoserMatching = parts.Distinct().Where(p => !(matches(p, losers).Any()));
            return wholes.Concat(anyNonLoserMatching);
        }

        /// <summary>
        /// Return a set of subparts of word, consecutive characters up to length 4.
        /// return set(word[i:i+n] for i in range(len(word)) for n in (1, 2, 3, 4)) 
        /// </summary>
        public static List<string> subparts(string input, int minChars = 1, int maxChars = 4)
        {
            var sets = new List<string>();
            foreach (var word in input.Split(' '))
            {
                for (int n = minChars; n <= maxChars; n++)
                {
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (i + n <= word.Length)
                        {
                            sets.Add(word.Substring(i, n));
                        }
                    }
                }
            }
            return sets;
        }

        /// <summary>
        /// Return all ways to replace a subset of chars in part with '.'. <para />
        /// if part == '': <para />
        ///    return {''}   <para />
        /// else: <para />
        ///    return {c+rest for rest in dotify(part[1:])  <para />
        ///            for c in replacements(part[0])} <para />
        /// </summary>
        public static List<string> dotify(string part)
        {
            var results = new List<string>();
            if (String.IsNullOrEmpty(part))
            {
                return results;
            }

            var start = part[0];
            foreach (var c in replacements(start))
            {
                if (part.Length > 1)
                {
                    var ending = part.Substring(1);
                    // ex: "this" loop through {"t", "."}
                    // ex: "his" recursively call and add peices as they come back.
                    foreach (var rest in dotify(ending))
                    {
                        // ex: "n." + "g"
                        results.Add(c + rest);
                    }
                }
                else
                {
                    results.Add(c.ToString());
                }
            }

            return results.Distinct().ToList();
        }



        /// <summary>
        /// Return replacement characters for char (char + '.' unless char is special).
        /// if (char == '^' or char == '$'):
        ///    return char
        /// else:
        ///    return char + '.'
        /// </summary>
        public static string replacements(char ch)
        {
            if (ch == '^' || ch == '$')
                return ch.ToString();
            else
                return ch + ".";
        }
    }
}
