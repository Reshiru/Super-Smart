using System.Linq;
using System.Security.Cryptography;

namespace SuperSmart.Core.Extension
{
    /// <summary>
    /// Represents the string extenstion class
    /// to add extra methods to the string class
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Generates a new hash value for the 
        /// given value with a given extra value (salt)
        /// </summary>
        /// <param name="value"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string GenerateHash(this string value, string salt)
        {
            var hash = value.GetHashCode().ToString();
            hash = hash + "sRaL*=" + $"{salt}";

            using (var crypto = new SHA512CryptoServiceProvider())
            {
                var bytes = new byte[hash.Length * sizeof(char)];
                System.Buffer.BlockCopy(hash.ToCharArray(), 0, bytes, 0, bytes.Length);

                var hashTwo = crypto.ComputeHash(bytes);

                return hashTwo.Aggregate("", (current, oneByte) => current + $"{oneByte}");
            }
        }
    }
}