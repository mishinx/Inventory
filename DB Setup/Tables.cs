using System;
using System.Data;
using System.Data.SqlClient;
using Bogus;
using Npgsql;
using BCrypt.Net;
using System.Drawing;
using System.Drawing.Imaging;

namespace DB_Setup
{
    public class Tables
    {
        public void Fill_Tables()
        {
            string connectionString = "Host=localhost;Port=5432;Database=inventarium;Username=postgres;Password=bochka2004;";
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    //FillAdministratorsTable(connection, 50);

                    //FillWarehousesTable(connection, 50);

                    FillGoodsTable(connection, 3);

                    //FillOperatorsTable(connection, 1);


                    Console.WriteLine("Данi успiшно доданi до бази даних.");
                    Console.WriteLine("Натиснiть будь-яку клавiшу для друку даних з таблиць...");
                    Console.ReadKey();
                    Console.WriteLine("");
                }


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
                            
                        string plainPassword = faker.Internet.Password();
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);
                        command.Parameters.AddWithValue("@password", hashedPassword);
                        command.Parameters.AddWithValue("@fullName", faker.Name.FullName());
                        command.Parameters.AddWithValue("@phone", faker.Phone.PhoneNumber());

                        command.ExecuteNonQuery();
                        if (i == 0) { Console.WriteLine(plainPassword); }
                    }
                }
            }

            static void FillWarehousesTable(NpgsqlConnection connection, int count)
            {
                var faker = new Bogus.Faker();
                using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO warehouses (addres, admin_id_ref) VALUES (@address, @adminId)", connection))
                {
                    for (int i = 0; i < count; i++)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@address", faker.Address.FullAddress());
                        command.Parameters.AddWithValue("@adminId", faker.Random.Number(1, count));

                        command.ExecuteNonQuery();
                    }
                }
            }

            static void FillGoodsTable(NpgsqlConnection connection, int count)
            {
                var faker = new Bogus.Faker();
                using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO goods (full_name, category, subcategory, short_description, quantity, price, warehouse_id_ref, photo) VALUES (@fullName, @category, @subcategory, @description, @quantity, @price, @warehouseId, @photo)", connection))
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
                        //command.Parameters.AddWithValue("@warehouseId", faker.Random.Number(1, count)); 
                        command.Parameters.AddWithValue("@warehouseId", 451); 
                        command.Parameters.AddWithValue("@photo", ImageConverter.ConvertImageToByteArray("./icons/goods.png", ImageFormat.Png));

                        command.ExecuteNonQuery();
                    }
                }
            }

            static void FillOperatorsTable(NpgsqlConnection connection, int count)
                {
                    var faker = new Bogus.Faker();
                    using (NpgsqlCommand command = new NpgsqlCommand("INSERT INTO operators (email_address, operator_password, full_name, phone_number, warehouse_id_ref, admin_id_ref, photo) VALUES (@email, @password, @fullName, @phone, @warehouseId, @adminId, @photo)", connection))
                    {
                        for (int i = 0; i < count; i++)
                        {
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@email", faker.Internet.Email()); 
                            
                            string plainPassword = faker.Internet.Password();
                            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);
                            command.Parameters.AddWithValue("@password", hashedPassword);
                            command.Parameters.AddWithValue("@fullName", faker.Name.FullName());
                            command.Parameters.AddWithValue("@phone", faker.Phone.PhoneNumber());
                            command.Parameters.AddWithValue("@warehouseId", faker.Random.Number(1, count));
                            command.Parameters.AddWithValue("@adminId", faker.Random.Number(1, count));
                            command.Parameters.AddWithValue("@photo", ImageConverter.ConvertImageToByteArray("./icons/employee_icon.png", ImageFormat.Png));
                            command.ExecuteNonQuery();
                            if (i == 0) { Console.WriteLine(plainPassword); }
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
            string connectionString = "Host=localhost;Port=5432;Database=inventarium;Username=postgres;Password=bochka2004;";

            try { 
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();

                Console.WriteLine("Данi з таблицi адміністраторів:");
                DisplayDataFromTable(connection, "administrators");

                Console.WriteLine("Данi з таблицi складів:");
                DisplayDataFromTable(connection, "warehouses");

                Console.WriteLine("Данi з таблицi операторів складів:");
                DisplayDataFromTable(connection, "operators");

                Console.WriteLine("Данi з таблицi товарів:");
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

    public class ImageConverter
    {
        public static byte[] ConvertImageToByteArray(string imagePath, ImageFormat format)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                using (Image image = Image.FromFile(imagePath))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, format);
                        return ms.ToArray();
                    }
                }
            }

            return null;
        }
    }
}