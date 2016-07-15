using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SentenceParser.Infrastructure
{
    public class ConfigHelper
    {
        public static string GetValue(string Key)
        {
            return ConfigurationManager.AppSettings[Key];
        }
    }
}