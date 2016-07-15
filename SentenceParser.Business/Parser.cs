using SentenceParser.Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SentenceParser.Business
{
    public class Parser
    {
        Func<string, bool> ExcludeFunc;
        public Parser(Func<string, bool> ExcludeLogic)
        {
            ExcludeFunc = ExcludeLogic;
        }
        public Result Parse(string sentence)
        {
            var wordList = sentence.Split(' ').AsEnumerable();

            if(ExcludeFunc != null)
            {
                wordList = wordList.Where(ExcludeFunc);
            }

            var wordCollection = wordList.GroupBy(word => word)
                                         .ToDictionary(group => group.Key, group => group.LongCount());

            var result = new Result();

            if(wordCollection.Count() == 0)
            {
                return result;
            }

            result.MaxCount = wordCollection.Values.Max();
            result.MinCount = wordCollection.Values.Min();

            var distinctCounts = wordCollection.Values.Distinct();

            result.MedianCount = distinctCounts.OrderBy(i => i).ElementAt(distinctCounts.Count() / 2);

            result.MaxOccuredWords = Filter(wordCollection, result.MaxCount);

            result.MinOccuredWords = Filter(wordCollection, result.MinCount);

            result.MedianOccuredWords = Filter(wordCollection, result.MedianCount);

            return result;
        }

        private string Filter(Dictionary<string, long> RecordDictionary, long Count)
        {
            return RecordDictionary.Where(i => i.Value == Count)
                                   .Select(i => i.Key)
                                   .Aggregate((a, b) => a + "," + b);
        }
    }
}
