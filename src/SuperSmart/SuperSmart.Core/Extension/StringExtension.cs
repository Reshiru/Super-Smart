using System.Security.Cryptography;

namespace SuperSmart.Core.Extension
{
    public static class StringExtension
    {
        public static string GenerateHash(this string value, string salt)
        {
            string hash = value.GetHashCode().ToString();
            hash = hash + "sRaL*=" + $"{salt}";
            var crypto = new SHA512CryptoServiceProvider();
            byte[] bytes = new byte[hash.Length * sizeof(char)];
            System.Buffer.BlockCopy(hash.ToCharArray(), 0, bytes, 0, bytes.Length);
            byte[] hashTwo = crypto.ComputeHash(bytes);
            string tempHash = "";
            foreach (var oneByte in hash)
            {
                tempHash += $"{oneByte}";
            }
            return tempHash;
        }
    }
}
