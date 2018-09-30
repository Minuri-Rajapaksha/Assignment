using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Shared.Extensions
{
    public static class Hash
    {
        public static string Sha256(this string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            using (var sha = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(input);
                var hash = sha.ComputeHash(bytes);

                return Convert.ToBase64String(hash);
            }
        }

        public static bool CompareHash(string plainText, string hashText)
        {
            return hashText.SequenceEqual(plainText.Sha256());
        }
    }
}
