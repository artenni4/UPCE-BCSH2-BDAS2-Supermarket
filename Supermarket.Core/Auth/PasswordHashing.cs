using System.Security.Cryptography;
using System.Text;

namespace Supermarket.Core.Auth
{
    internal static class PasswordHashing
    {
        /// <summary>
        /// Generates random salt for a hash
        /// </summary>
        /// <returns></returns>
        public static byte[] GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            using (var provider = RandomNumberGenerator.Create())
            {
                provider.GetBytes(saltBytes);
            }
            return saltBytes;
        }

        /// <summary>
        /// Generates salted hash from given string
        /// </summary>
        /// <param name="password">string containing password</param>
        /// <param name="salt">salt for hashing, generated automatically if not provided</param>
        /// <returns></returns>
        public static byte[] GenerateSaltedHash(string password, byte[]? salt = null)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            salt ??= GenerateSalt();

            byte[] combinedBytes = new byte[passwordBytes.Length + salt.Length];
            Array.Copy(passwordBytes, 0, combinedBytes, 0, passwordBytes.Length);
            Array.Copy(salt, 0, combinedBytes, passwordBytes.Length, salt.Length);

            return SHA256.HashData(combinedBytes);
        }

        public static bool HashesAreEqual(byte[] hash1, byte[] hash2) => CryptographicOperations.FixedTimeEquals(hash1, hash2);
    }
}
