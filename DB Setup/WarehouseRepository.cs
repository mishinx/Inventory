using Inventory_Context;

namespace DB
{
    public class WarehouseRepository
    {
        private readonly InventoryContext _context = new InventoryContext();

        public Warehouse Create(Warehouse warehouse)
        {
            _context.warehouses.Add(warehouse);
            _context.SaveChanges();
            return warehouse;
        }

        public Warehouse GetWarehouseById(int warehouseId)
        {
            return _context.warehouses.Find(warehouseId);
        }

        public Warehouse GetWarehouseByAddress(string address)
        {
            return _context.warehouses.FirstOrDefault(w => w.addres == address);
        }

        public List<Warehouse> GetWarehousesForAdministrator(int administratorId)
        {
            var warehousesForAdministrator = _context.warehouses
                .Where(w => w.admin_id_ref == administratorId)
                .Distinct()
                .ToList();

            return warehousesForAdministrator;
        }

    }
}