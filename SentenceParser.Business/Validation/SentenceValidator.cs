using SentenceParser.Business.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentenceParser.Business.Validation
{
    public class SentenceValidator
    {
        public ValidationResult Validate(string sentence, int maxSize)
        {
            var result = new ValidationResult();

            if(string.IsNullOrWhiteSpace(sentence))
            {
                result.ErrorMessage.Add("Sentence cannot be empty");
            }

            if((Encoding.ASCII.GetByteCount(sentence) / 1048576) > maxSize)
            {
                result.ErrorMessage.Add("Sentence size exceeded the limits");
            }

            result.IsValid = result.ErrorMessage.Count == 0;

            return result;
        }
    }
}
