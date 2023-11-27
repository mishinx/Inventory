using Bogus;
using Bogus.DataSets;
using BusinessLogic;
using DB;
using DB_Setup;
using Inventory_Context;
using Moq;
using System.Drawing.Imaging;

namespace BLL_Tests
{
    [TestClass]
    public class WarehouseServiceTests
    {
        [TestMethod]
        public void CreateWarehouse_ReturnsWarehouseOnSuccess()
        {
            var faker = new Bogus.Faker();

            var mockWarehouseRepository = new Mock<WarehouseRepository>();
            var warehouseService = new WarehouseService(mockWarehouseRepository.Object);
            var warehouse = new Warehouse { addres = faker.Address.FullAddress(), admin_id_ref = faker.Random.Number(1000) };

            mockWarehouseRepository.Setup(r => r.Create(It.IsAny<Warehouse>())).Returns(warehouse);

            var result = warehouseService.CreateWarehouse(warehouse);

            Assert.IsNotNull(result);
            Assert.AreEqual(warehouse, result);
        }

        [TestMethod]
        public void CreateWarehouse_ReturnsNullOnError()
        {
            var faker = new Bogus.Faker();

            var mockWarehouseRepository = new Mock<WarehouseRepository>();
            var warehouseService = new WarehouseService(mockWarehouseRepository.Object);
            var warehouse = new Warehouse { addres = faker.Address.FullAddress(), admin_id_ref = faker.Random.Number(1000) };

            mockWarehouseRepository.Setup(r => r.Create(It.IsAny<Warehouse>())).Throws(new Exception("Simulated error"));

            var result = warehouseService.CreateWarehouse(warehouse);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetWarehouseById_ReturnsWarehouse()
        {
            var faker = new Bogus.Faker();

            var mockWarehouseRepository = new Mock<WarehouseRepository>();
            var warehouseService = new WarehouseService(mockWarehouseRepository.Object);
            var warehouseId = 1;
            var expectedWarehouse = new Warehouse { addres = faker.Address.FullAddress(), admin_id_ref = faker.Random.Number(1000) };

            mockWarehouseRepository.Setup(r => r.GetWarehouseById(warehouseId)).Returns(expectedWarehouse);

            var result = warehouseService.GetWarehouseById(warehouseId);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedWarehouse, result);
        }

        [TestMethod]
        public void GetWarehouseByAddress_ReturnsWarehouse()
        {
            var faker = new Bogus.Faker();

            var mockWarehouseRepository = new Mock<WarehouseRepository>();
            var warehouseService = new WarehouseService(mockWarehouseRepository.Object);
            var address = "123 Main St";
            var expectedWarehouse = new Warehouse { addres = "123 Main St", admin_id_ref = faker.Random.Number(1000) };

            mockWarehouseRepository.Setup(r => r.GetWarehouseByAddress(address)).Returns(expectedWarehouse);

            var result = warehouseService.GetWarehouseByAddress(address);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedWarehouse, result);
        }

        [TestMethod]
        public void GetWarehousesForAdministrator_ReturnsWarehouses()
        {
            var faker = new Bogus.Faker();
            var mockWarehouseRepository = new Mock<WarehouseRepository>();
            var warehouseService = new WarehouseService(mockWarehouseRepository.Object);
            var administratorId = 1;
            var expectedWarehouses = new List<Warehouse> { new Warehouse { addres = faker.Address.FullAddress(), admin_id_ref = 1 }, new Warehouse { addres = faker.Address.FullAddress(), admin_id_ref = 1 } };

            mockWarehouseRepository.Setup(r => r.GetWarehousesForAdministrator(administratorId)).Returns(expectedWarehouses);

            var result = warehouseService.GetWarehousesForAdministrator(administratorId);

            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(expectedWarehouses, result);
        }
    }

    [TestClass]
    public class AdministratorServiceTests
    {
        [TestMethod]
        public void GetAdministratorByEmail_ReturnsAdministrator()
        {
            var mockRepository = new Mock<AdministratorRepository>();
            var service = new AdministratorService(mockRepository.Object);
            string email = "admin@example.com";
            var expectedAdministrator = new Administrator { email_address = email };
            mockRepository.Setup(r => r.GetAdministratorByEmail(email)).Returns(expectedAdministrator);

            var result = service.GetAdministratorByEmail(email);

            Assert.AreEqual(expectedAdministrator, result);
        }

        [TestMethod]
        public void GetAdministratorsByCompanyName_ReturnsListOfAdministrators()
        {
            var mockRepository = new Mock<AdministratorRepository>();
            var service = new AdministratorService(mockRepository.Object);
            string companyName = "Example Company";
            var expectedAdministrators = new List<Administrator> { new Administrator { company_name = companyName, email_address = "admin@example.com" }, new Administrator { company_name = companyName, email_address = "admin2@example.com" }, new Administrator { company_name = companyName, email_address = "admin3@example.com" } };
            mockRepository.Setup(r => r.GetAdministratorsByCompanyName(companyName)).Returns(expectedAdministrators);

            var result = service.GetAdministratorsByCompanyName(companyName);

            CollectionAssert.AreEqual(expectedAdministrators, result);
        }

        [TestMethod]
        public void GetFilteredAdmins_ReturnsFilteredAdministrators()
        {
            var mockRepository = new Mock<AdministratorRepository>();
            var service = new AdministratorService(mockRepository.Object);
            string searchTerm = "John";
            string companyName = "Example Company";
            var expectedAdministrators = new List<Administrator> { new Administrator { company_name = companyName, full_name = "John Doe" }, new Administrator { company_name = companyName, full_name = "Peter Johnson"}, new Administrator { company_name = companyName, full_name = "Frederick John Peterson" } };

            mockRepository.Setup(r => r.GetFilteredAdmins(searchTerm, companyName)).Returns(expectedAdministrators);

            var result = service.GetFilteredAdmins(searchTerm, companyName);

            CollectionAssert.AreEqual(expectedAdministrators, result);
        }

        [TestMethod]
        public void RegisterAdministrator_ReturnsTrueOnSuccess()
        {
            var mockRepository = new Mock<AdministratorRepository>();
            var service = new AdministratorService(mockRepository.Object);
            string companyName = "Example Company";
            string email = "admin@example.com";
            string password = "password";
            bool expectedReturnValue = true;

            mockRepository.Setup(r => r.Create(It.IsAny<Administrator>())).Returns(expectedReturnValue);

            var result = service.RegisterAdministrator(companyName, email, password);

            Assert.AreEqual(expectedReturnValue, result);
        }

        [TestMethod]
        public void RegisterAdministrator_ReturnsFalseOnError()
        {
            var mockRepository = new Mock<AdministratorRepository>();
            var service = new AdministratorService(mockRepository.Object);

            mockRepository.Setup(r => r.Create(It.IsAny<Administrator>())).Returns(false);

            bool result = service.RegisterAdministrator("Example Company", "example@gmail.com", "password");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RegisterAdministrator_ThrowsExceptionOnError()
        {
            var mockRepository = new Mock<AdministratorRepository>();
            var service = new AdministratorService(mockRepository.Object);

            mockRepository.Setup(r => r.Create(It.IsAny<Administrator>())).Throws(new Exception("Simulated error"));

            Assert.ThrowsException<Exception>(() =>
            {
                service.RegisterAdministrator("Example Company", "example@gmail.com", "password");
            });
        }

        [TestMethod]
        public void UpdateAdministrator_CallsRepositoryUpdate()
        {
            var mockRepository = new Mock<AdministratorRepository>();
            var service = new AdministratorService(mockRepository.Object);
            var administratorToUpdate = new Administrator { company_name = "Example Company", full_name = "John Doe", email_address = "admin@example.com" };

            service.UpdateAdministrator(administratorToUpdate);

            mockRepository.Verify(r => r.Update(administratorToUpdate), Times.Once);
        }
    }

    [TestClass]
    public class OperatorServiceTests
    {
        [TestMethod]
        public void GetOperatorByEmail_ReturnsOperator()
        {
            var mockRepository = new Mock<OperatorRepository>();
            var service = new OperatorService(mockRepository.Object);
            string email = "operator@example.com";
            var expectedOperator = new Operator { email_address = email };

            mockRepository.Setup(r => r.GetOperatorByEmail(email)).Returns(expectedOperator);

            var result = service.GetOperatorByEmail(email);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedOperator, result);
        }

        [TestMethod]
        public void GetAllOperatorsForAdministrator_ReturnsListOfOperators()
        {
            var mockRepository = new Mock<OperatorRepository>();
            var service = new OperatorService(mockRepository.Object);
            int adminId = 1;
            var expectedOperators = new List<Operator> { new Operator { admin_id_ref = adminId, full_name = "John" }, new Operator { admin_id_ref = adminId, full_name = "Anton" } };

            mockRepository.Setup(r => r.GetAllOperatorsForAdministrator(adminId)).Returns(expectedOperators);

            var result = service.GetAllOperatorsForAdministrator(adminId);

            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(expectedOperators, result);
        }

        [TestMethod]
        public void GetFilteredOperatorsForAdministrator_ReturnsListOfOperators()
        {
            var mockRepository = new Mock<OperatorRepository>();
            var service = new OperatorService(mockRepository.Object); 
            int adminId = 1;
            string searchTerm = "John";
            var expectedOperators = new List<Operator> { new Operator { admin_id_ref = adminId, full_name = "John Doe" }, new Operator { admin_id_ref = adminId, full_name = "Peter Johnson" } };
            
            mockRepository.Setup(r => r.GetFilteredOperatorsForAdministrator(searchTerm, adminId)).Returns(expectedOperators);

            var result = service.GetFilteredOperatorsForAdministrator(searchTerm, adminId);

            Assert.IsNotNull(result);
            CollectionAssert.AreEqual(expectedOperators, result);
        }

        [TestMethod]
        public void RegisterOperator_ReturnsTrueOnSuccess()
        {
            var mockRepository = new Mock<OperatorRepository>();
            var service = new OperatorService(mockRepository.Object);
            var expectedOperator = new Operator { email_address = "operator@example.com", operator_password = "password", full_name = "John Doe", admin_id_ref = 1, warehouse_id_ref = 1 };

            mockRepository.Setup(r => r.Create(It.IsAny<Operator>())).Returns(true);

            var result = service.RegisterOperator("operator@example.com", "password", "John Doe", 1, 1);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RegisterOperator_ReturnsFalseOnError()
        {
            var mockRepository = new Mock<OperatorRepository>();
            var service = new OperatorService(mockRepository.Object);

            mockRepository.Setup(r => r.Create(It.IsAny<Operator>())).Returns(false);

            var result = service.RegisterOperator("operator@example.com", "password", "John Doe", 1, 1);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpdateOperator_CallsRepositoryUpdate()
        {
            var mockRepository = new Mock<OperatorRepository>();
            var service = new OperatorService(mockRepository.Object);
            var operatorToUpdate = new Operator { email_address = "operator@example.com", operator_password = "password", full_name = "John Doe", admin_id_ref = 1, warehouse_id_ref = 1 };

            service.UpdateOperator(operatorToUpdate);

            mockRepository.Verify(r => r.Update(operatorToUpdate), Times.Once);
        }
    }

    [TestClass]
    public class GoodsServiceTests
    {
        [TestMethod]
        public void CreateGoods_CallsRepositoryCreate()
        {
            var faker = new Bogus.Faker();
            var mockRepository = new Mock<GoodsRepository>();
            var service = new GoodsService(mockRepository.Object);
            var goods = new Goods { full_name = faker.Commerce.ProductName(), category = faker.Commerce.Categories(1)[0], subcategory = faker.Commerce.Categories(1)[0], short_description = faker.Lorem.Sentence(), quantity = faker.Random.Number(1, 100), price = faker.Random.Decimal(1, 1000), photo = ImageConverter.ConvertImageToByteArray("./icons/goods.png", ImageFormat.Png)};

            service.CreateGoods(goods);

            mockRepository.Verify(r => r.Create(goods), Times.Once);
        }

        [TestMethod]
        public void GetAllGoodsForAdministrator_ReturnsGoodsList()
        {
            var faker = new Bogus.Faker();
            var mockRepository = new Mock<GoodsRepository>();
            var service = new GoodsService(mockRepository.Object);
            var adminId = 1;
            var expectedGoodsList = new List<Goods> { new Goods { full_name = faker.Commerce.ProductName(), category = faker.Commerce.Categories(1)[0], subcategory = faker.Commerce.Categories(1)[0], short_description = faker.Lorem.Sentence(), quantity = faker.Random.Number(1, 100), price = faker.Random.Decimal(1, 1000), photo = ImageConverter.ConvertImageToByteArray("./icons/goods.png", ImageFormat.Png) },
            new Goods { full_name = faker.Commerce.ProductName(), category = faker.Commerce.Categories(1)[0], subcategory = faker.Commerce.Categories(1)[0], short_description = faker.Lorem.Sentence(), quantity = faker.Random.Number(1, 100), price = faker.Random.Decimal(1, 1000), photo = ImageConverter.ConvertImageToByteArray("./icons/goods.png", ImageFormat.Png) },
            new Goods { full_name = faker.Commerce.ProductName(), category = faker.Commerce.Categories(1)[0], subcategory = faker.Commerce.Categories(1)[0], short_description = faker.Lorem.Sentence(), quantity = faker.Random.Number(1, 100), price = faker.Random.Decimal(1, 1000), photo = ImageConverter.ConvertImageToByteArray("./icons/goods.png", ImageFormat.Png) },
            };

            mockRepository.Setup(r => r.GetAllGoodsForAdministrator(adminId)).Returns(expectedGoodsList);

            var result = service.GetAllGoodsForAdministrator(adminId);

            CollectionAssert.AreEqual(expectedGoodsList, result);
        }

        [TestMethod]
        public void GetAllGoodsForOperator_ReturnsGoodsList()
        {
            var faker = new Bogus.Faker();
            var mockRepository = new Mock<GoodsRepository>();
            var mockRepository2 = new Mock<OperatorRepository>();
            var service = new GoodsService(mockRepository.Object);
            var service2 = new OperatorService(mockRepository2.Object);
            var operatorId = 1; 
            var expectedGoodsList = new List<Goods> { new Goods { full_name = faker.Commerce.ProductName(), category = faker.Commerce.Categories(1)[0], subcategory = faker.Commerce.Categories(1)[0], short_description = faker.Lorem.Sentence(), quantity = faker.Random.Number(1, 100), price = faker.Random.Decimal(1, 1000), photo = ImageConverter.ConvertImageToByteArray("./icons/goods.png", ImageFormat.Png), warehouse_id_ref = 2 },
            new Goods { full_name = faker.Commerce.ProductName(), category = faker.Commerce.Categories(1)[0], subcategory = faker.Commerce.Categories(1)[0], short_description = faker.Lorem.Sentence(), quantity = faker.Random.Number(1, 100), price = faker.Random.Decimal(1, 1000), photo = ImageConverter.ConvertImageToByteArray("./icons/goods.png", ImageFormat.Png), warehouse_id_ref = 2 }
            };

            service2.RegisterOperator("example@gmail.com", "password", "John Doe", 2, 3);
            mockRepository.Setup(r => r.GetAllGoodsForOperator(operatorId)).Returns(expectedGoodsList);

            var result = service.GetAllGoodsForOperator(operatorId);

            CollectionAssert.AreEqual(expectedGoodsList, result);
        }

        [TestMethod]
        public void UpdateGoods_CallsRepositoryUpdate()
        {
            var faker = new Bogus.Faker();
            var mockRepository = new Mock<GoodsRepository>();
            var service = new GoodsService(mockRepository.Object);

            var updatedGoods = new Goods
            {
                goods_id = 1,
                full_name = faker.Commerce.ProductName(),
                category = faker.Commerce.Categories(1)[0],
                subcategory = faker.Commerce.Categories(1)[0],
                short_description = faker.Lorem.Sentence(),
                quantity = faker.Random.Number(1, 100),
                price = faker.Random.Decimal(1, 1000),
                photo = ImageConverter.ConvertImageToByteArray("./icons/goods.png", ImageFormat.Png)
            };


            service.UpdateGoods(updatedGoods);

            mockRepository.Verify(r => r.Update(updatedGoods), Times.Once);
        }

        [TestMethod]
        public void DeleteGoods_CallsRepositoryDelete()
        {
            var faker = new Bogus.Faker();
            var mockRepository = new Mock<GoodsRepository>();
            var service = new GoodsService(mockRepository.Object);
            var goodsId = 1;
            var goods = new Goods { goods_id = goodsId, full_name = faker.Commerce.ProductName(), category = faker.Commerce.Categories(1)[0], subcategory = faker.Commerce.Categories(1)[0], short_description = faker.Lorem.Sentence(), quantity = faker.Random.Number(1, 100), price = faker.Random.Decimal(1, 1000), photo = ImageConverter.ConvertImageToByteArray("./icons/goods.png", ImageFormat.Png) };

            service.CreateGoods(goods);

            service.DeleteGoods(goodsId);

            mockRepository.Verify(r => r.Delete(goodsId), Times.Once);
        }

        [TestMethod]
        public void GetFilteredGoods_ReturnsFilteredGoodsList()
        {
            var faker = new Bogus.Faker();
            var mockRepository = new Mock<GoodsRepository>();
            var service = new GoodsService(mockRepository.Object);
            var searchTerm = "example";
            var expectedFilteredGoodsList = new List<Goods> { new Goods { full_name = "example product", category = faker.Commerce.Categories(1)[0], subcategory = faker.Commerce.Categories(1)[0], short_description = faker.Lorem.Sentence(), quantity = faker.Random.Number(1, 100), price = faker.Random.Decimal(1, 1000), photo = ImageConverter.ConvertImageToByteArray("./icons/goods.png", ImageFormat.Png) },new Goods { full_name = "examplee product", category = faker.Commerce.Categories(1)[0], subcategory = faker.Commerce.Categories(1)[0], short_description = faker.Lorem.Sentence(), quantity = faker.Random.Number(1, 100), price = faker.Random.Decimal(1, 1000), photo = ImageConverter.ConvertImageToByteArray("./icons/goods.png", ImageFormat.Png) },
            new Goods { full_name = "product for example", category = faker.Commerce.Categories(1)[0], subcategory = faker.Commerce.Categories(1)[0], short_description = faker.Lorem.Sentence(), quantity = faker.Random.Number(1, 100), price = faker.Random.Decimal(1, 1000), photo = ImageConverter.ConvertImageToByteArray("./icons/goods.png", ImageFormat.Png) },
            new Goods { full_name = "exampled product", category = faker.Commerce.Categories(1)[0], subcategory = faker.Commerce.Categories(1)[0], short_description = faker.Lorem.Sentence(), quantity = faker.Random.Number(1, 100), price = faker.Random.Decimal(1, 1000), photo = ImageConverter.ConvertImageToByteArray("./icons/goods.png", ImageFormat.Png) }
            };
            mockRepository.Setup(r => r.GetFilteredGoods(searchTerm)).Returns(expectedFilteredGoodsList);

            var result = service.GetFilteredGoods(searchTerm);

            CollectionAssert.AreEqual(expectedFilteredGoodsList, result);
        }

        [TestMethod]
        public void GetCategoriesForAdministrator_ReturnsCategoriesList()
        {
            var mockRepository = new Mock<GoodsRepository>();
            var service = new GoodsService(mockRepository.Object);
            var adminId = 1;
            var expectedCategoriesList = new List<string> { "dsf", " sdf"};
            mockRepository.Setup(r => r.GetCategoriesForAdministrator(adminId)).Returns(expectedCategoriesList);

            var result = service.GetCategoriesForAdministrator(adminId);

            CollectionAssert.AreEqual(expectedCategoriesList, result);
        }

        [TestMethod]
        public void GetSubcategoriesForAdministrator_ReturnsSubcategoriesList()
        {
            var mockRepository = new Mock<GoodsRepository>();
            var service = new GoodsService(mockRepository.Object);
            var adminId = 1;
            var category = "example";
            var expectedSubcategoriesList = new List<string> { "jlh"};
            mockRepository.Setup(r => r.GetSubcategoriesForAdministrator(adminId, category)).Returns(expectedSubcategoriesList);

            var result = service.GetSubCategoriesForAdministrator(adminId, category);

            CollectionAssert.AreEqual(expectedSubcategoriesList, result);
        }
    }
}