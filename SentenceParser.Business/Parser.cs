using Microsoft.VisualBasic.FileIO;
using SentenceParser.Business.DTO;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SentenceParser.Business
{
    public class Parser
    {
        Stream _fileStream;

        public Parser(Stream inputStream)
        {
            _fileStream = inputStream;
        }

        /// <summary>
        /// Reads the stream Line by line
        /// </summary>
        /// <returns>Array of word get by delimited by ' '(space)</returns>
        private IEnumerable<string[]> Readline()
        {
            using(var textFieldParser = new TextFieldParser(_fileStream))
            {
                textFieldParser.TextFieldType = FieldType.Delimited;
                textFieldParser.Delimiters = new[] { " " };
                textFieldParser.HasFieldsEnclosedInQuotes = true;

                while(!textFieldParser.EndOfData)
                {
                    yield return textFieldParser.ReadFields();
                }
            }
        }

        /// <summary>
        /// Counts the unique words in a sentence by ignoring the word in exclude logic.
        /// </summary>
        /// <returns>Words of Max,Min and Median occurrence with their count.</returns>
        public Result CountWords()
        {
            // To Hold the Unique Words with Count.
            var wordCollection = new Dictionary<string, long>();

            //Reset the stream position, before start reading.
            _fileStream.Position = 0;

            foreach(var item in Readline())
            {
                foreach(var word in item)
                {
                    if(NotInExcludeLogic(word))  //Check for exclude logic.
                    {
                        if(wordCollection.ContainsKey(word))
                        {
                            wordCollection[word]++;
                        }
                        else
                        {
                            wordCollection.Add(word, 1);
                        }
                    }
                }
            }

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="RecordDictionary"></param>
        /// <param name="Count"></param>
        /// <returns>Returns the Comma separated string of the Record collection based on the Count Value passed.</returns>
        private string Filter(Dictionary<string, long> RecordDictionary, long Count)
        {
            return RecordDictionary.Where(i => i.Value == Count)
                                   .Select(i => i.Key)
                                   .Aggregate((a, b) => a + "," + b);
        }

        /// <summary>
        /// Returns false if the word input word matches any of the exclusion pattern
        /// </summary>
        /// <param name="word"></param>
        /// <returns>Returns false if the word input word matches any of the exclusion pattern</returns>
        private static bool NotInExcludeLogic(string word)
        {
            //Matches 1234-5678-9012-3456, 1234567890123456
            if(Regex.IsMatch(word, @"^[0-9]{4}([\-]?[0-9]{4}){3}$"))
            {
                return false;
            }

            //Matches Password expression that requires one lower case letter, one upper case letter, one digit, 4-14 length.
            if(Regex.IsMatch(word, @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{4,14}$"))
            {
                return false;
            }
            return true;
        }
    }
}
