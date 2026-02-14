using System.Text.RegularExpressions;

namespace UsersService.Helpers
{
    public static class PasswordValidator
    {
        public static bool IsStrongPassword(string password)
        {
            string pattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";
            return Regex.IsMatch(password, pattern);
        }
    }
}