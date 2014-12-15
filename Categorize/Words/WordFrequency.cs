using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace Categorize.Words
{    
    static class WordFrequency
    {
        static void Main(string[] args)
        {
            string filePath = args[0];
            MatchCollection matchCollection = StripPunctuationAndMatchWords(filePath);
            
            WordCollection collection = CountWords(matchCollection);

            Console.WriteLine("unique words : " + collection.UniqueWords.Count);
            Console.WriteLine("total words  : " + collection.TotalWords);

            IO.FileHelper.CreateReport("C:\\report.csv", collection);
        }

        

        private static MatchCollection StripPunctuationAndMatchWords(string filePath)
        {
            string fullBook = IO.FileHelper.GetStringFromFile(filePath);
            //remove numbers and punctuation
            fullBook = Regex.Replace(fullBook, "\\.|;|:|,|[0-9]|’", string.Empty);
            //create collection of words
            MatchCollection matchCollection = Regex.Matches(fullBook, @"[\w]+", RegexOptions.Multiline);
            //create linked list for words
            return matchCollection;
        }

        private static WordCollection CountWords(MatchCollection wordCollection)
        {
            var result = new WordCollection();

            //populate wordList with content of collection
            for (int i = 0; i < wordCollection.Count; i++)
            {
                var word = wordCollection[i].ToString().ToLower().Trim();
                if (result.Words.ContainsKey(word))
                { result.Words[word]++; }
                else
                { result.Words.Add(word, 1); }
            }
            return result;
        }

        
    }
}