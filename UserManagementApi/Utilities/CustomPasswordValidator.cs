using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;

namespace UserManagementApi.Utilities
{
    public static class CustomPasswordValidator
    {
        public static bool IsValid(string password, out string errorMessage)
        {
            Regex specialCharacterRegex = new ("[^A-Za-z0-9]");

            if (password.Length < 8)
            {
                errorMessage = "Password is to short";
                return false;
            }

            if (password.Length > 30)
            {
                errorMessage = "Password is to long";
                return false;
            }

            if (!password.Any(char.IsUpper))
            {
                errorMessage = "Password must include at least one uppercase letter.";
                return false;
            }

            if (!password.Any(char.IsLower))
            {
                errorMessage = "Password must include at least one lower letter.";
                return false;
            }

            if (!password.Any(char.IsDigit))
            {
                errorMessage = "Password must include at least one number.";
                return false;
            }

            if (password.Any(char.IsWhiteSpace))
            {
                errorMessage = "Password must not contain spaces.";
                return false;
            }

            if (!specialCharacterRegex.IsMatch(password))
            {
                errorMessage = "Password must include at least one special character.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
