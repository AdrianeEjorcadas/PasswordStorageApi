using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace UserManagementApi.Helpers
{
    public class HashingHelper
    {
        //Generate Salt or Token
        public static byte[] GenerateSalt(int size)
        {
            var salt = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        //Hash the combine password and salt
        public static string HashPassword(string password, byte[] salt, int iterations = 4, int memorySize = 65536, int degreeOfParallelism = 2)
        {
            #region sha-256
            //using (var sha256 = SHA256.Create())
            //{
            //    // Combine the password and the salt
            //    var saltedPassword = Encoding.UTF8.GetBytes(password);
            //    var passwordWithSalt = new byte[saltedPassword.Length + salt.Length];
            //    salt.CopyTo(passwordWithSalt, 0);
            //    saltedPassword.CopyTo(passwordWithSalt, salt.Length);

            //    // Compute the hash
            //    var hash = sha256.ComputeHash(passwordWithSalt);
            //    return Convert.ToBase64String(hash);
            //}
            #endregion

            using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)))
            {
                // Configure Argon2 parameters
                argon2.Salt = salt;
                argon2.Iterations = iterations;
                argon2.MemorySize = memorySize; // in KB
                argon2.DegreeOfParallelism = degreeOfParallelism;

                // Generate the hash
                byte[] hashBytes = argon2.GetBytes(32); // 32-byte hash
                return Convert.ToBase64String(hashBytes);
            }
        }

        //Hash token
        public static string HashToken(byte[] token)
        {
            using (var sha256 = SHA256.Create())
            {
                // Compute the hash
                var hash = sha256.ComputeHash(token);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
