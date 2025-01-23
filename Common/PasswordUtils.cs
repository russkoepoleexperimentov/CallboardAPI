using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public static class PasswordUtils
    {
        public static string Hashify(string source)
        {
            // https://stackoverflow.com/questions/3984138/hash-string-in-c-sharp
            return Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(source)));
        }

        public static bool Compare(string password, string hash)
        {
            return Hashify(password) == hash;
        }


    }
}
