using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Categorize.Words
{
    class WordCollection
    {
        public WordCollection()
        {
            Words = new Dictionary<string, int>();
        }
        public Dictionary<string, int> Words;
        public int AverageWordLength
        {
            get
            {
                return Words.Sum(pair => pair.Key.Length) / TotalWords;
            }
        }
        public int TotalWords
        {
            get
            {
                return Words.Count();
            }
        }
        public List<string> UniqueWords
        {
            get
            {
                return Words
                    .Where(pair => pair.Value == 1)
                    .Select(p => p.Key)
                    .ToList();
            }
        }
        public string MostUsedWord
        {
            get
            {
                var maximum = Words.Values.Max();
                return Words.First(pair => pair.Value == maximum).Key;
            }
        }
    }
}
