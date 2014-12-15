using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Categorize;

namespace CategorizeTest
{
    [TestClass]
    public class MetaRegexTest
    {

        public List<string> presidentialWinners = Program.CleanAndSplitMultiLine(@"washington adams jefferson jefferson madison madison monroe 
    monroe adams jackson jackson van-buren harrison polk taylor pierce buchanan 
    lincoln lincoln grant grant hayes garfield cleveland harrison cleveland mckinley
    mckinley roosevelt taft wilson wilson harding coolidge hoover roosevelt 
    roosevelt roosevelt roosevelt truman eisenhower eisenhower kennedy johnson nixon 
    nixon carter reagan reagan bush clinton clinton bush bush obama obama");

        public List<string> presidentialLosers = Program.CleanAndSplitMultiLine(@"clinton jefferson adams pinckney pinckney clinton king adams 
    jackson adams clay van-buren van-buren clay cass scott fremont breckinridge 
    mcclellan seymour greeley tilden hancock blaine cleveland harrison bryan bryan 
    parker bryan roosevelt hughes cox davis smith hoover landon wilkie dewey dewey 
    stevenson stevenson nixon goldwater humphrey mcgovern ford carter mondale 
    dukakis bush dole gore kerry mccain romney");

        [TestMethod]
        public void PresidentsTest()
        {
            presidentialLosers.RemoveAll(name => presidentialWinners.Contains(name));
            presidentialLosers.Remove("fremont");

            var xkcd = "bu|[rn]t|[coy]e|[mtg]a|j|iso|n[hl]|[ae]d|lev|sh|[lnd]i|[po]o|ls";
            Debug.WriteLine("verify Presidents: {0}", Categorize.Reg.MetaRegex.VerifyMatchesAllWinnersNoLosers(xkcd, presidentialWinners, presidentialLosers));

            var solution = Categorize.Reg.MetaRegex.findRegex(presidentialWinners, presidentialLosers);
            Debug.WriteLine("Presidents Regex: {0}", new[]{solution});
            Debug.Assert(solution == "i..n|oo|a.a|i..o|j|sh|a.t|i.l|r.e$|ay.|lev|u..n|di|po|nn");
            Debug.WriteLine("len Randal: {0}",xkcd.Length );
            Debug.WriteLine("len Wills: {0}", solution.Length);
        }

        [TestMethod]
        public void SubPartsTest()
        {
            var answer1 = new List<string> { "^", "i", "t", "$", "^i", "it", "t$", "^it", "it$", "^it$" };
            Program.StringListsEqual(Categorize.Reg.MetaRegex.subparts("^it$"), answer1);

            var answer2 = new List<string> { "t", "h", "i", "s", "th", "hi", "is", "thi", "his", "this" };
            Program.StringListsEqual(Categorize.Reg.MetaRegex.subparts("this"), answer2);
        }

        [TestMethod]
        public void DotifyTest()
        {
            var list1 = new List<string> { "it", "i.", ".t", ".." };
            Debug.Assert(Program.StringListsEqual(Categorize.Reg.MetaRegex.dotify("it"), list1));

            var list2 = new List<string> { "^it$", "^i.$", "^.t$", "^..$" };
            Debug.Assert(Program.StringListsEqual(Categorize.Reg.MetaRegex.dotify("^it$"), list2));

            var list3 = new List<string>{"this", "thi.", "th.s", "th..", "t.is", "t.i.", "t..s", "t...",
                              ".his", ".hi.", ".h.s", ".h..", "..is", "..i.", "...s", "...."};
            Debug.Assert(Program.StringListsEqual(Categorize.Reg.MetaRegex.dotify("this"), list3));            
        }

        [TestMethod]
        public void ReplacementsTest()
        {
            Debug.Assert(Categorize.Reg.MetaRegex.replacements('x') == "x.");
            Debug.Assert(Categorize.Reg.MetaRegex.replacements('^') == "^");
            Debug.Assert(Categorize.Reg.MetaRegex.replacements('$') == "$");
        }

        [TestMethod]
        public void RegexComponentsTest()
        {
            List<string> components1 = Categorize.Reg.MetaRegex.matchAtLeastOneWinnerButNoLosers(new List<string> { "win" }, 
                new List<string> { "losers", "bin", "won" }).ToList();
            List<string> answer1 = new List<string>{
                "^win$", "^win", "^wi.", "wi.",  "wi", "^wi", "win$", "win", "wi.$"};
            Program.StringListsEqual(components1, answer1);

            List<string> components2 = Categorize.Reg.MetaRegex.matchAtLeastOneWinnerButNoLosers(new List<string> { "win" },
                new List<string> { "losers", "bin", "won", "wine" }).ToList();
            List<string> answer2 = new List<string> { "^win$", "win$", "wi.$" };
            Program.StringListsEqual(components2, answer2);
        }

        [TestMethod]
        public void MatchesTest()
        {
            Debug.Assert(Program.StringListsEqual(
                Categorize.Reg.MetaRegex.matches("a|b|c", new List<string> { "a", "b", "c", "d", "e" }),
                new List<string> { "a", "b", "c" }));
            Debug.Assert(Program.StringListsEqual(
                Categorize.Reg.MetaRegex.matches("a|b|c", new List<string> { "any", "bee", "succeed", "dee", "eee!" }),
                new List<string>{"any", "bee", "succeed"}));
        }

        [TestMethod]
        public void VerifyTest()
        {
            Debug.Assert(Categorize.Reg.MetaRegex.VerifyMatchesAllWinnersNoLosers("a+b+", new List<string> { "ab", "aaabb" }, new List<string> { "a", "bee", "a b" }));
        }

        [TestMethod]
        public void FindRegexTest()
        {
            var allThese = new List<string> { "ahahah", "ciao"};
            var noneOfThese = new List<string> { "ahaha", "bye" };
            var answer1 = Categorize.Reg.MetaRegex.findRegex(allThese, noneOfThese);
            Debug.Assert(answer1 == "a.$");
        }

        [TestMethod]
        public void ORTest()
        {
            Debug.Assert(Categorize.Reg.MetaRegex.OR(new List<string> { "a", "b", "c" }) == "a|b|c");
            Debug.Assert(Categorize.Reg.MetaRegex.OR(new List<string> { "a" }) == "a");
        }

        [TestMethod]
        public void scoreTest()
        {
            var reg = "a.$";
            var reg2 = "i|h$";
            var reg3 = "c|h";
            var allThese = new List<string> { "ahahah", "ciao", "vanadiam", "paraboliac" };
            Debug.Assert(Categorize.Reg.MetaRegex.score(reg, allThese) == 13);
            Debug.Assert(Categorize.Reg.MetaRegex.score(reg2, allThese) == 12);
            Debug.Assert(Categorize.Reg.MetaRegex.score(reg3, allThese) == 9);
        }


        //Debug.Assert(Program.words("This is a TEST this is") == new List<string>{"this", "is", "a", "test"});
        //Debug.Assert(Program.lines("Testing / 1 2 3 / Testing over") == new List<string>{"TESTING", "1 2 3", "TESTING OVER"});
    }
}