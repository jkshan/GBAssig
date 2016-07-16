using SentenceParser.Business;
using SentenceParser.Business.Validation;
using SentenceParser.Infrastructure;
using System;
using System.IO;
using System.Linq;
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
        public ActionResult Parse(HttpPostedFileBase file)
        {
            try
            {
                if(file == null || file.ContentLength == 0)
                {
                    ViewBag.ErrorMessage = "Please Select Valid File";
                    return View();
                }

                if(!file.FileName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    ViewBag.ErrorMessage = "File Type Not Supported";
                    return View();
                }

                if((file.InputStream.Length) > Convert.ToInt32(ConfigHelper.GetValue("MaxSize")))
                {
                    ViewBag.ErrorMessage = "File size exceeded the allowed limit";
                    return View();
                }

                string sentence = new StreamReader(file.InputStream).ReadToEnd();

                var logger = new LogHelper();
                logger.WriteLog(sentence);

                var Validator = new SentenceValidator();

                var validationResult = Validator.Validate(sentence);

                sentence = null;

                if(validationResult.IsValid)
                {
                    var parser = new Parser(file.InputStream);
                    var result = parser.CountWords();
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
    }
}
