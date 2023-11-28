using Inventory_Context;
using Serilog;

namespace DB
{
    public class WarehouseRepository
    {
        private readonly InventoryContext _context = new InventoryContext();
        ILogger _logger = LoggerManager.Instance.Logger;

        public virtual Warehouse Create(Warehouse warehouse)
        {
            _logger.Information("Створено склад - " + warehouse.addres);
            _context.warehouses.Add(warehouse);
            _context.SaveChanges();
            return warehouse;
        }

        public virtual Warehouse GetWarehouseById(int warehouseId)
        {
            _logger.Information("Запит на отримання складу за ID - " + warehouseId.ToString());
            return _context.warehouses.Find(warehouseId);
        }

        public virtual Warehouse GetWarehouseByAddress(string address)
        {
            _logger.Information("Запит на отримання складу за адресою - " + address);
            return _context.warehouses.FirstOrDefault(w => w.addres == address);
        }

        public virtual List<Warehouse> GetWarehousesForAdministrator(int administratorId)
        {
            _logger.Information("Запит на отримання всіх складів для адміністратора - " + administratorId.ToString());
            var warehousesForAdministrator = _context.warehouses
                .Where(w => w.admin_id_ref == administratorId)
                .Distinct()
                .ToList();

            return warehousesForAdministrator;
        }
    }
}