namespace BusinessLogic
{
    public class AuthenticationService
    {
        private readonly AdministratorService _administratorService;
        private readonly OperatorService _operatorService;

        public AuthenticationService(AdministratorService administratorService, OperatorService operatorService)
        {
            _administratorService = administratorService;
            _operatorService = operatorService;
        }

        public bool[] AuthenticateUser(string email, string password)
        {
            var administrator = _administratorService.GetAdministratorByEmail(email);
            var _operator = _operatorService.GetOperatorByEmail(email);

            if (administrator != null)
            {
                if (PasswordHasher.VerifyPassword(administrator.admin_password, password))
                {
                    return new bool[] { true, true };
                }
                else { return new bool[] { true, false }; }
            }
            else if (_operator != null)
            {
                if (PasswordHasher.VerifyPassword(_operator.operator_password, password))
                {
                    return new bool[] { false, true };
                }
                else { return new bool[] { false, false }; }
            }
            else { return null; }
        }
    }

    public class PasswordHasher
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool VerifyPassword(string hashedPassword, string candidatePassword)
        {
            return BCrypt.Net.BCrypt.Verify(candidatePassword, hashedPassword);
        }
    }
}
