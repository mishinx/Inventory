using BusinessLogic;
using Inventory_Context;

public class AdministratorService
{
    private readonly AdministratorRepository _administratorRepository;

    public AdministratorService(AdministratorRepository administratorRepository)
    {
        _administratorRepository = administratorRepository;
    }

    public Administrator GetAdministratorByEmail(string email)
    {
        return _administratorRepository.GetAdministratorByEmail(email);
    }

    public List<Administrator> GetAdministratorsByCompanyName(string company_name)
    {
        return _administratorRepository.GetAdministratorsByCompanyName(company_name);
    }

    public List<Administrator> GetFilteredAdmins(string searchTerm, string company_name)
    {
        return _administratorRepository.GetFilteredAdmins(searchTerm, company_name);
    }

    public bool RegisterAdministrator(string companyName, string email, string password)
    {
        string hashedPassword = PasswordHasher.HashPassword(password);

        var newAdmin = new Administrator
        {
            company_name = companyName,
            email_address = email,
            admin_password = hashedPassword,
            full_name = "New user name",
            phone_number = "+380680000000"
        };

        return _administratorRepository.Create(newAdmin);
    }

    public void UpdateAdministrator(Administrator administrator)
    {
        _administratorRepository.Update(administrator);
    }
}
