using Npgsql;

namespace DB_Setup
{
    public class SetupScript
    {
        public void SetupingScript()
        {
            string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=*******;";
            try
            {
                // Читання SQL-скрипта з файлу
                string script = File.ReadAllText("createdb.sql");

                // Встановлення з'єднання з PostgreSQL
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Виконання SQL-скрипта для створення бази даних
                    using (NpgsqlCommand createDbCommand = new NpgsqlCommand("CREATE DATABASE Inventarium;", connection))
                    {
                        createDbCommand.ExecuteNonQuery();
                        Console.WriteLine("Базу даних 'inventarium' створено успiшно.");
                    }

                    // Закриття з'єднання
                    connection.Close();
                }
                connectionString = "Host=localhost;Port=5432;Database=inventarium;Username=postgres;Password=*******;";
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    // Виконання SQL-скрипта для створення таблиць у базі даних 'Inventarium'
                    using (NpgsqlCommand command = new NpgsqlCommand(script, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Таблиці створено успішно.");
                    }

                    // Закриття з'єднання
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка: " + ex.Message);
            }
        }
    }
}
