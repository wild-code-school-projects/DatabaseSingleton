using DatabaseSingleton.Singleton;
using System.Data.SqlClient;

namespace DatabaseSingleton.Repositories
{
    internal class ProductRepository
    {
        private Database _database;

        public ProductRepository()
        {
            _database = Database.GetInstance();
        }

        public void ShowAllProducts()
        {
            string query = "SELECT * FROM products";
            using (SqlDataReader reader = _database.ExecuteQuery(query))
            {
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["id"]}, {reader["name"]}, {reader["price"]}");
                }
            }
        }

        public void AddProduct(string name, decimal price)
        {
            // Vérifier si le produit existe déjà
            string checkIfExistsQuery = $"SELECT COUNT(*) FROM products WHERE Name = '{name}'";
            int existingCount = (int)_database.ExecuteNonQuery(checkIfExistsQuery);

            if (existingCount == 0)
            {
                // Insérer le produit uniquement s'il n'existe pas déjà
                string insertQuery = $"INSERT INTO products (Name, Price) VALUES ('{name}', {price})";
                int rowsAffected = _database.ExecuteNonQuery(insertQuery);
                Console.WriteLine($"{rowsAffected} row(s) inserted.");
            }
            else
            {
                Console.WriteLine($"Product '{name}' already exists. Skipping insertion.");
            }
        }
    }
}
