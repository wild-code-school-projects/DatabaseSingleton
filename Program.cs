using DatabaseSingleton.Repositories;

namespace DatabaseSingleton
{
    internal class Program
    {
        static void Main(string[] args)
        {
            UserRepository userRepository = new UserRepository();
            ProductRepository productRepository = new ProductRepository();

            Console.WriteLine("Users:");
            userRepository.ShowAllUsers();

            Console.WriteLine("Products:");
            productRepository.ShowAllProducts();

            userRepository.AddUser("John Doe");

            productRepository.AddProduct("Laptop", 999.99m);
        }
    }
}
