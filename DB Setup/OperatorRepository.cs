using Inventory_Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace DB
{
    public class OperatorRepository
    {
        private readonly InventoryContext _context = new InventoryContext();
        ILogger _logger = LoggerManager.Instance.Logger;

        public virtual Operator GetOperatorByEmail(string email)
        {
            _logger.Information("Запит на оператора - " + email);
            return _context.operators.FirstOrDefault(o => o.email_address == email);
        }

        public virtual List<Operator> GetAllOperatorsForAdministrator(int adminId)
        {
            _logger.Information("Запит на отримання операторів за ID адміністратора - " + adminId);
            List<Operator> operatorsForAdmin = _context.operators
                .Where(o => o.admin_id_ref == adminId)
                .ToList();

            return operatorsForAdmin;
        }

        public virtual List<Operator> GetFilteredOperatorsForAdministrator(string searchTerm, int adminId)
        {
            _logger.Information("Повернуто операторів за запитом - " + searchTerm);
            return _context.operators.Where(o => o.admin_id_ref == adminId && o.full_name.Contains(searchTerm))
                .ToList();
        }

        public virtual bool Create(Operator _operator)
        {
            bool userExists = _context.operators.Any(o => o.email_address == _operator.email_address);

            if (!userExists && _operator != null)
            {
                _logger.Information("Створено оператора складу - " + _operator.email_address);
                _context.operators.Add(_operator);
                _context.SaveChanges();
                return true;
            }

            _logger.Information("Помилка створення оператора");
            return false;
        }

        public virtual void Update(Operator _operator)
        {
            _logger.Information("Оновлено оператора складу - " + _operator.email_address);
            _context.Entry(_operator).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
