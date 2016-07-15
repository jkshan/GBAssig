using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
