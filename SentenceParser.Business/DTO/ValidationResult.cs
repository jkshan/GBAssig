using System.Collections.Generic;

namespace SentenceParser.Business.DTO
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            ErrorMessage = new List<string>();
        }
        public bool IsValid { get; set; }
        public List<string> ErrorMessage { get; set; }
    }
}
