using System.Text;

namespace API_Automation.Helpers
{
    public class BasicAuth
    {
        public static string BasicAuthValue(string username, string password)
        {
            string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1")
                       .GetBytes(username + ":" + password));

            return encoded;
        }
    }
}
