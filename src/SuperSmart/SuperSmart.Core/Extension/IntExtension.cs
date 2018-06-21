using System;
using System.Linq;

namespace SuperSmart.Core.Extension
{
    /// <summary>
    /// The int extensions to add
    /// extra functionality to the int class
    /// </summary>
    public static class IntExtension
    {
        private static Random random = new Random();

        /// <summary>
        /// Generates a random string for the given length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomString(this int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-*+";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
