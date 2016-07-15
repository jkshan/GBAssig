using SentenceParser.Business;
using SentenceParser.Business.DTO;
using SentenceParser.Business.Validation;
using SentenceParser.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SentenceParser.Controllers
{
    [Authorize]
    public class SentenceParserController : Controller
    {
        public ActionResult Parse()
        {
            return View();
        }

        // POST: SentenceParser/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Parse(FormCollection collection)
        {
            try
            {
                string sentence = collection["sentence"];

                var logger = new LogHelper();
                logger.WriteLog(sentence);

                var Validator = new SentenceValidator();

                var validationResult = Validator.Validate(sentence, Convert.ToInt32(ConfigHelper.GetValue("MaxSizeInMB")));

                if(validationResult.IsValid)
                {
                    var parser = new Parser(ExcludeLogic);
                    var result = parser.Parse(sentence);
                    return View("Success", result);
                }
                else
                {
                    ViewBag.ErrorMessage = validationResult.ErrorMessage.Aggregate((a, b) => a + "," + b);
                    return View();
                }
            }
            catch(Exception ex)
            {
                ViewBag.ErrorMessage = "Exception occurred, please contact admin";
                return View();
            }
        }

        [NonAction]
        public bool ExcludeLogic(string word)
        {
            //Matches 1234-5678-9012-3456, 1234 5678 9012 3456, 1234567890123456
            var cardCheck = new Regex(@"^[0-9]{4}([\-\s]?[0-9]{4}){3}$");

            if(cardCheck.IsMatch(word))
            {
                return false;
            }
            //Matches Password expression that requires one lower case letter, one upper case letter, one digit, 4-14 length.
            var passWord = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{4,14}$");

            if(passWord.IsMatch(word))
            {
                return false;
            }
            return true;
        }
    }
}
