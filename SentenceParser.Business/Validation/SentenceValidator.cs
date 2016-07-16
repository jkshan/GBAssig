using SentenceParser.Business.DTO;

namespace SentenceParser.Business.Validation
{
    public class SentenceValidator
    {
        public ValidationResult Validate(string sentence)
        {
            var result = new ValidationResult();

            if(string.IsNullOrWhiteSpace(sentence))
            {
                result.ErrorMessage.Add("Sentence cannot be empty");
            }

            result.IsValid = result.ErrorMessage.Count == 0;

            return result;
        }
    }
}
