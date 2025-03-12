using System.Security.Cryptography;

namespace PasswordStorageApi.Helpers
{
    public static class SaltHelper
    {
        public static byte[] GenerateSalt(int size)
        {
            var salt = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
