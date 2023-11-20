using Inventory_Context;
using DB;

namespace BusinessLogic
{
    public class WarehouseService
    {
        private readonly WarehouseRepository _warehouseRepository;

        public WarehouseService(WarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public Warehouse CreateWarehouse(Warehouse warehouse)
        {
            try
            {
                return _warehouseRepository.Create(warehouse);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка при створенні складу: " + ex.Message);
                return null;
            }
        }

        public Warehouse GetWarehouseById(int warehouseId)
        {
            return _warehouseRepository.GetWarehouseById(warehouseId);
        }

        public Warehouse GetWarehouseByAddress(string address)
        {
            return _warehouseRepository.GetWarehouseByAddress(address);
        }

        public List<Warehouse> GetWarehousesForAdministrator(int administratorId)
        {
            return _warehouseRepository.GetWarehousesForAdministrator(administratorId);
        }
    }
}