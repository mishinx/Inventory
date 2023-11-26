using Inventory_Context;
using Microsoft.EntityFrameworkCore;

namespace DB
{
    public class OperatorRepository
    {
        private readonly InventoryContext _context = new InventoryContext();

        public Operator GetOperatorByEmail(string email)
        {
            return _context.operators.FirstOrDefault(o => o.email_address == email);
        }

        public List<Operator> GetAllOperatorsForAdministrator(int adminId)
        {
            List<Operator> operatorsForAdmin = _context.operators
                .Where(o => o.admin_id_ref == adminId)
                .ToList();

            return operatorsForAdmin;
        }

        public List<Operator> GetFilteredOperatorsForAdministrator(string searchTerm, int adminId)
        {
            return _context.operators.Where(o => o.admin_id_ref == adminId && o.full_name.Contains(searchTerm))
                .ToList();
        }

        public bool Create(Operator _operator)
        {
            bool userExists = _context.operators.Any(o => o.email_address == _operator.email_address);

            if (!userExists && _operator != null)
            {
                _context.operators.Add(_operator);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public void Update(Operator _operator)
        {
            _context.Entry(_operator).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
