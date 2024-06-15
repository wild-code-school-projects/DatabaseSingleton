using DatabaseSingleton.Singleton;
using System.Data.SqlClient;

namespace DatabaseSingleton.Repositories
{
    internal class UserRepository
    {
        private Database _database;

        public UserRepository()
        {
            _database = Database.GetInstance();
        }

        public void ShowAllUsers()
        {
            string query = "SELECT * FROM users";
            using (SqlDataReader reader = _database.ExecuteQuery(query))
            {
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["id"]}, {reader["name"]}");
                }
            }
        }

        public void AddUser(string name)
        {
            string checkIfExistsQuery = $"SELECT COUNT(*) FROM users WHERE Name = '{name}'";
            int existingCount = (int)_database.ExecuteNonQuery(checkIfExistsQuery);

            if (existingCount == 0)
            {
                string insertQuery = $"INSERT INTO users (Name) VALUES ('{name}')";
                int rowsAffected = _database.ExecuteNonQuery(insertQuery);
                Console.WriteLine($"{rowsAffected} row(s) inserted.");
            }
            else
            {
                Console.WriteLine($"User '{name}' already exists. Skipping insertion.");
            }
        }
    }
}
