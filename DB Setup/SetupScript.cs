using Npgsql;

namespace DB_Setup
{
    public class SetupScript
    {
        public void SetupingScript()
        {
            string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=12345678;";
            try
            {
                string script = File.ReadAllText("createdb.sql");

                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (NpgsqlCommand createDbCommand = new NpgsqlCommand("CREATE DATABASE Inventarium;", connection))
                    {
                        createDbCommand.ExecuteNonQuery();
                        Console.WriteLine("Базу даних 'inventarium' створено успiшно.");
                    }

                    connection.Close();
                }
                connectionString = "Host=localhost;Port=5432;Database=inventarium;Username=postgres;Password=12345678;";
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();

                    using (NpgsqlCommand command = new NpgsqlCommand(script, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Таблиці створено успішно.");
                    }

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
