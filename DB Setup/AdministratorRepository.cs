using DB;
using Inventory_Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

public class AdministratorRepository
{
    private readonly InventoryContext _context = new InventoryContext();
    ILogger _logger = LoggerManager.Instance.Logger;

    public virtual Administrator GetAdministratorByEmail(string email)
    {
        _logger.Information("Запит на отримання адміністратора за логіном - " + email);
        return _context.administrators.FirstOrDefault(a => a.email_address == email);
    }

    public virtual List<Administrator> GetAdministratorsByCompanyName(string company_name)
    {
        _logger.Information("Запит на отримання адміністраторів за назвою компанії - " + company_name);
        return _context.administrators.Where(a => a.company_name == company_name).ToList();
    }

    public virtual List<Administrator> GetFilteredAdmins(string searchTerm, string company_name)
    {
        _logger.Information("Запит на фільтрацію адміністратора за назвою компанії - " + company_name + " та умовою " + searchTerm);
        return _context.administrators.Where(a => a.company_name == company_name && a.full_name.Contains(searchTerm))
            .ToList();
    }

    public virtual bool Create(Administrator administrator)
    {
        bool userExists = _context.administrators.Any(a => a.email_address == administrator.email_address);

        if (!userExists && administrator != null)
        {
            _logger.Information("Створено адміністратора - " + administrator.email_address);
            _context.administrators.Add(administrator);
            _context.SaveChanges();
            return true;
        }

        _logger.Error("Помилка створення адміністратора");
        return false;
    }

    public virtual void Update(Administrator administrator)
    {
        _logger.Information("Оновлено адміністратора - " + administrator.email_address);
        _context.Entry(administrator).State = EntityState.Modified;
        _context.SaveChanges();
    }
}