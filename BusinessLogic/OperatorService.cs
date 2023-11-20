using DB;
using Inventory_Context;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic
{
    public class OperatorService
    {
        private readonly OperatorRepository _operatorRepository;

        public OperatorService(OperatorRepository operatorRepository)
        {
            _operatorRepository = operatorRepository;
        }

        public Operator GetOperatorByEmail(string email)
        {
            return _operatorRepository.GetOperatorByEmail(email);
        }
        public List<Operator> GetAllOperatorsForAdministrator(int adminId)
        {
            return _operatorRepository.GetAllOperatorsForAdministrator(adminId);
        }

        public List<Operator> GetFilteredOperatorsForAdministrator(string searchTerm, int adminId)
        {
            return _operatorRepository.GetFilteredOperatorsForAdministrator(searchTerm, adminId);
        }

        public bool RegisterOperator(string email, string password, string fullName, int _warehouse_id_ref, int _admin_id_ref)
        {
            string hashedPassword = PasswordHasher.HashPassword(password);
            string relativeImagePath = "icons/employee_icon.png";
            byte[] defaultImageBytes = null;
            defaultImageBytes = File.ReadAllBytes(relativeImagePath);
            
            var newOperator = new Operator
            {
                full_name = fullName,
                email_address = email,
                operator_password = hashedPassword,
                warehouse_id_ref = _warehouse_id_ref,
                admin_id_ref = _admin_id_ref,
                phone_number = "+380000000000",
                photo = defaultImageBytes
            };

            return _operatorRepository.Create(newOperator);
        }
        public void UpdateOperator(Operator _operator)
        {
            _operatorRepository.Update(_operator);
        }
    }
}
