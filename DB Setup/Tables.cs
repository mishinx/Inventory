using System;
using System.Data;
using System.Data.SqlClient;
using Bogus;
using Npgsql;

namespace DB_Setup
{
    public class Tables
    {
        public void Fill_Tables()
        {
            // Рядок підключення до новоствореної БД в PostgreSQL
            string connectionString = "Host=localhost;Port=5432;Database=inventarium;Username=postgres;Password=*******;";
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Заповнення таблиці administrators
                FillAdministratorsTable(connection, 50);

                // Заповнення таблиці warehouses
                FillWarehousesTable(connection, 50);

                // Заповнення таблиці goods
                FillGoodsTable(connection, 50);

                // Заповнення таблиці operators
                FillOperatorsTable(connection, 50);


                Console.WriteLine("Данi успiшно доданi до бази даних.");
                Console.WriteLine("Натиснiть будь-яку клавiшу для друку даних з таблиць...");
                Console.ReadKey();
                Console.WriteLine("");
            }


            // Функція заповнення таблиці адміністраторів
            static void FillAdministratorsTable(NpgsqlConnection connection, int count)
            {
                var faker = new Bogus.Faker();
                using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO administrators (company_name, email_address, admin_password, full_name, phone_number) VALUES (@companyName, @email, @password, @fullName, @phone)", connection))
                {
                    for (int i = 0; i < count; i++)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@companyName", faker.Company.CompanyName());
                        command.Parameters.AddWithValue("@email", faker.Internet.Email());
                        command.Parameters.AddWithValue("@password", faker.Internet.Password());
                        command.Parameters.AddWithValue("@fullName", faker.Name.FullName());
                        command.Parameters.AddWithValue("@phone", faker.Phone.PhoneNumber());

                        command.ExecuteNonQuery();
                    }
                }
            }

            // Функція заповнення таблиці складів
            static void FillWarehousesTable(NpgsqlConnection connection, int count)
            {
                var faker = new Bogus.Faker();
                using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO warehouses (addres, admins_id) VALUES (@address, @adminId)", connection))
                {
                    for (int i = 0; i < count; i++)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@address", faker.Address.FullAddress());
                        command.Parameters.AddWithValue("@adminId", faker.Random.Number(1, count)); // Випадковий admin_id

                        command.ExecuteNonQuery();
                    }
                }
            }

            // Функція заповнення таблиці товарів
            static void FillGoodsTable(NpgsqlConnection connection, int count)
            {
                var faker = new Bogus.Faker();
                using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO goods (full_name, category, subcategory, short_description, quantity, price, warehouses_id) VALUES (@fullName, @category, @subcategory, @description, @quantity, @price, @warehouseId)", connection))
                {
                    for (int i = 0; i < count; i++)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@fullName", faker.Commerce.ProductName());
                        command.Parameters.AddWithValue("@category", faker.Commerce.Categories(1)[0]);
                        command.Parameters.AddWithValue("@subcategory", faker.Commerce.Categories(1)[0]);
                        command.Parameters.AddWithValue("@description", faker.Lorem.Sentence());
                        command.Parameters.AddWithValue("@quantity", faker.Random.Number(1, 100));
                        command.Parameters.AddWithValue("@price", faker.Random.Decimal(1, 1000));
                        command.Parameters.AddWithValue("@warehouseId", faker.Random.Number(1, count)); // Випадковий warehouse_id
                                                                                                        //command.Parameters.AddWithValue("@photo", null); // Фото можна додати як bytea, але це поза обсягом цього прикладу

                        command.ExecuteNonQuery();
                    }
                }
            }

            // Функція заповнення таблиці операторів
            static void FillOperatorsTable(NpgsqlConnection connection, int count)
                {
                    var faker = new Bogus.Faker();
                    using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO operators (email_address, admin_password, full_name, phone_number, warehouses_id, admins_id) VALUES (@email, @password, @fullName, @phone, @warehouseId, @adminId)", connection))
                    {
                        for (int i = 0; i < count; i++)
                        {
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@email", faker.Internet.Email());
                            command.Parameters.AddWithValue("@password", faker.Internet.Password());
                            command.Parameters.AddWithValue("@fullName", faker.Name.FullName());
                            command.Parameters.AddWithValue("@phone", faker.Phone.PhoneNumber());
                            command.Parameters.AddWithValue("@warehouseId", faker.Random.Number(1, count)); // Випадковий warehouse_id
                            command.Parameters.AddWithValue("@adminId", faker.Random.Number(1, count)); // Випадковий admin_id

                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка: " + ex.Message);
            }
        }
        public void Print_Tables()
        {
            string connectionString = "Host=localhost;Port=5432;Database=inventarium;Username=postgres;Password=*******;";

            try { 
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                // Виведення всіх даних з таблиці administrators
                Console.WriteLine("Данi з таблицi administrators:");
                DisplayDataFromTable(connection, "administrators");

                // Виведення всіх даних з інших таблиць аналогічним чином
                Console.WriteLine("Данi з таблицi warehouses:");
                DisplayDataFromTable(connection, "warehouses");

                Console.WriteLine("Данi з таблицi operators:");
                DisplayDataFromTable(connection, "operators");

                Console.WriteLine("Данi з таблицi goods:");
                DisplayDataFromTable(connection, "goods");

                connection.Close();
            }

            Console.WriteLine("Натиснiть будь-яку клавiшу для завершення...");
            Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка: " + ex.Message);
            }
        }
        static void DisplayDataFromTable(NpgsqlConnection connection, string tableName)
        {
            using (NpgsqlCommand command = new NpgsqlCommand($"SELECT * FROM {tableName}", connection))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.WriteLine($"{reader.GetName(i)}: {reader[i]}");
                        }
                        Console.WriteLine("---------------");
                    }
                }
            }
        }
    }
}