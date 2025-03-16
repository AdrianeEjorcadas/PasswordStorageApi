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
        public static string HashPassword(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                // Combine the password and the salt
                var saltedPassword = Encoding.UTF8.GetBytes(password);
                var passwordWithSalt = new byte[saltedPassword.Length + salt.Length];
                salt.CopyTo(passwordWithSalt, 0);
                saltedPassword.CopyTo(passwordWithSalt, salt.Length);

                // Compute the hash
                var hash = sha256.ComputeHash(passwordWithSalt);
                return Convert.ToBase64String(hash);
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
