using MySql.Data.MySqlClient;

namespace OnlineRetailersStore.Data
{
    public static class OnlineRetailersStoreContext
    {
        public static string ConnectionString = "server=localhost;port=3306;database=online_retailers_store;userid=root;password=root";
    }

    public static class DatabaseContext
    {
        private static string password = System.IO.File.ReadAllText(@"C:\MySqlPassword\Password.txt");
        public static string ConnectionString = $"server=localhost;port=3306;userid=root;password={password}";
    }
}
