using System.Data.SqlClient;

namespace DatabaseSingleton.Singleton
{
    internal class Database
    {
        private static Database _instance = null;

        private const string CONNECTION_STRING_MASTER = "Data Source=localhost;Database=master;Integrated Security=false;User ID=sa;Password=Toto123*;Encrypt=true;TrustServerCertificate=true;";
        private string CONNECTION_STRING_SINGLETONDATA = "Data Source=localhost;Database=SingletonData;Integrated Security=false;User ID=sa;Password=Toto123*;Encrypt=true;TrustServerCertificate=true;";


        public static Database GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Database();
                _instance.CreateDatabaseIfNotExists("SingletonData");
                _instance.CreateUsersTable();
                _instance.CreateProductsTable();
            }

            return _instance;
        }



        public void OpenConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        public SqlDataReader ExecuteQuery(string query)
        {
            OpenConnection();
            SqlCommand command = new SqlCommand(query, _connection);
            return command.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }

        public int ExecuteNonQuery(string query)
        {
            OpenConnection();
            SqlCommand command = new SqlCommand(query, _connection);
            int result = command.ExecuteNonQuery();
            CloseConnection();
            return result;
        }

        private Database()
        {
            _connection = new SqlConnection(CONNECTION_STRING_MASTER);
        }


        private void CreateDatabaseIfNotExists(string databaseName)
        {
            string checkDbQuery = $"IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'{databaseName}') CREATE DATABASE [{databaseName}]";
            ExecuteNonQueryOnMaster(checkDbQuery);

            // Update connection string to use the new database
            CONNECTION_STRING_SINGLETONDATA = $"Data Source=localhost;Database={databaseName};Integrated Security=false;User ID=sa;Password=Toto123*;Encrypt=true;TrustServerCertificate=true;";
            _connection.ConnectionString = CONNECTION_STRING_SINGLETONDATA;

            // Ensure the user 'sa' has access to the new database
            string grantPermissionQuery = $"USE [{databaseName}]; ALTER AUTHORIZATION ON DATABASE::[{databaseName}] TO [sa];";
            ExecuteNonQueryOnMaster(grantPermissionQuery);
        }

        private void ExecuteNonQueryOnMaster(string query)
        {
            using (SqlConnection connection = new SqlConnection(CONNECTION_STRING_MASTER))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private void CreateUsersTable()
        {
            string createTableQuery = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'users')
            CREATE TABLE Users (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Name NVARCHAR(100) NOT NULL,
                Email NVARCHAR(100) NULL
            )";
            ExecuteNonQuery(createTableQuery);
        }

        private void CreateProductsTable()
        {
            string createTableQuery = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'products')
            CREATE TABLE Products (
                Id INT PRIMARY KEY IDENTITY(1,1),
                Name NVARCHAR(100) NOT NULL,
                Price DECIMAL(18,2) NOT NULL
            )";
            ExecuteNonQuery(createTableQuery);
        }

        private SqlConnection _connection;
    }
}
