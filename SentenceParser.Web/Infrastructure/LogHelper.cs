using System;
using System.IO;
using System.Text;
using System.Web;

namespace SentenceParser.Infrastructure
{
    public class LogHelper
    {
        string _path, _fileName;

        public LogHelper() : this(HttpContext.Current.Server.MapPath("Logs"), DateTime.Now.ToString("dd-MM-yyyy") + ".txt")
        {
        }
        public LogHelper(string Path, string FileName)
        {
            _path = Path;
            _fileName = FileName;
        }
        public void WriteLog(string message)
        {
            try
            {
                if(!string.IsNullOrEmpty(message))
                {
                    if(!Directory.Exists(_path))
                    {
                        Directory.CreateDirectory(_path);
                    }

                    if(!_path.EndsWith("\\"))
                    {
                        _path += "\\";
                    }

                    using(StreamWriter sw = new StreamWriter(string.Concat(_path, _fileName), true, Encoding.UTF8))
                    {
                        sw.WriteLine(string.Format("[{0}]: {1}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), message));
                        sw.Close();
                    }
                }
            }
            catch(Exception ex)
            {
            }
        }
    }
}