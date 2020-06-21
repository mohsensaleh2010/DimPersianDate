using System.Collections.Specialized;
using System.Configuration;

namespace DateDim
{
    public class AppConfig
    {
        public static string GetConnectionString()
        {
            var result = ConfigurationManager.ConnectionStrings["MainConnectionString"].ConnectionString;
            return result;
        }
        public static NameValueCollection AppSettings => ConfigurationManager.AppSettings;
    }
}
