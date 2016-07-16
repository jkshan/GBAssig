using System.Configuration;

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