using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace BE.LocalAccountabilitySystem.Common.Security
{
    /// <summary>
    /// A static utility for password hashing and security.
    /// </summary>
    public static class SecurityUtil
    {
        private const int BYTE_COUNT = 128;

        /// <summary>
        /// Uses <see cref="RandomNumberGenerator"/> to create a byte array to be used
        /// as a password salt.
        /// </summary>
        /// <returns>A cryptographically secure byte array.</returns>
        public static byte[] GenerateSalt()
        {
            byte[] secureBytes = RandomNumberGenerator.GetBytes(BYTE_COUNT);

            return secureBytes;
        }

        /// <summary>
        /// Hashes a password with the given <paramref name="salt"/> byte array using
        /// Argon2id.
        /// </summary>
        /// <param name="password">Plain text string password.</param>
        /// <param name="salt">Byte array generated via <see cref="GenerateSalt"/></param>
        /// <returns>Hashed password byte array.</returns>
        public static byte[] HashPassword(string password, byte[] salt)
        {
            byte[] passwordBytes = Encoding.Unicode.GetBytes(password);

            var argon2id = new Argon2id(passwordBytes)
            {
                DegreeOfParallelism = 16, // double the amount of cores.
                MemorySize = 8192, // memory in KB.
                Iterations = 4, // 4 is a conservative amount. Some documentation suggests 40.
                Salt = salt
            };

            var hash = argon2id.GetBytes(BYTE_COUNT);

            return hash;
        }

        /// <summary>
        /// Checks whether or not a string password matches a hashed password.
        /// </summary>
        /// <param name="password">Plain text password.</param>
        /// <param name="salt">The byte array used as the original salt.</param>
        /// <param name="hash">The stored hashed password.</param>
        /// <returns></returns>
        public static bool VerifyHash(string password, byte[] salt, byte[] hash)
        {
            var hashedPass = HashPassword(password, salt);
            return hashedPass.SequenceEqual(hash);
        }

        public static byte[] HashToken(string token)
        {
            byte[] tokenBytes = Encoding.Unicode.GetBytes(token);

            var argon2id = new Argon2id(tokenBytes)
            {
                DegreeOfParallelism = 16, // double the amount of cores.
                MemorySize = 8192, // memory in KB.
                Iterations = 4, // 4 is a conservative amount. Some documentation suggests 40.
            };

            var hash = argon2id.GetBytes(BYTE_COUNT);

            return hash;
        }
    }
}
