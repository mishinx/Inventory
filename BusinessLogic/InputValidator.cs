using System.Text.RegularExpressions;

namespace BusinessLogic
{
    public class InputValidator
    {
        public static bool IsCompanyNameValid(string companyName)
        {
            if (string.IsNullOrWhiteSpace(companyName) || companyName.Length < 2 || companyName.Length > 18)
                return false;
            string companyNamePattern = "^[a-zA-Z& ]*$";

            return Regex.IsMatch(companyName, companyNamePattern);
        }

        public static bool IsEmailValid(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string emailPattern = @"^[a-zA-Z0-9._%+-]{3,20}@[a-zA-Z0-9.-]{2,20}\.[a-zA-Z]{2,10}$";

            return Regex.IsMatch(email, emailPattern);
        }

        public static bool IsPasswordValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;

            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,20}$";

            return Regex.IsMatch(password, passwordPattern);
        }
    }
}
