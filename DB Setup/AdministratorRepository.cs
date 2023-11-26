using Inventory_Context;
using Microsoft.EntityFrameworkCore;

public class AdministratorRepository
{
    private readonly InventoryContext _context = new InventoryContext();

    public virtual Administrator GetAdministratorByEmail(string email)
    {

        return _context.administrators.FirstOrDefault(a => a.email_address == email);
    }

    public virtual List<Administrator> GetAdministratorsByCompanyName(string company_name)
    {
        return _context.administrators.Where(a => a.company_name == company_name).ToList();
    }

    public virtual List<Administrator> GetFilteredAdmins(string searchTerm, string company_name)
    {
        return _context.administrators.Where(a => a.company_name == company_name && a.full_name.Contains(searchTerm))
            .ToList();
    }

    public virtual bool Create(Administrator administrator)
    {
        bool userExists = _context.administrators.Any(a => a.email_address == administrator.email_address);

        if (!userExists && administrator != null)
        {
            _context.administrators.Add(administrator);
            _context.SaveChanges();
            return true;
        }

        return false;
    }

    public virtual void Update(Administrator administrator)
    {
        _context.Entry(administrator).State = EntityState.Modified;
        _context.SaveChanges();
    }
}
