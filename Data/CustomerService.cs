using MySql.Data.MySqlClient;
using OnlineRetailersStore.Data.Models;
using System;

namespace OnlineRetailersStore.Data
{
    public static class CustomerService
    {
        public static string GetCustomerIdByUsernameAndPassword(string username, string password)
        {
            string customerId = string.Empty;

            using (var conn = new MySqlConnection(OnlineRetailersStoreContext.ConnectionString))
            {
                conn.OpenAsync();
                var sql = $"SELECT * " +
                    $"FROM online_retailers_store.customer " +
                    $"WHERE Username = '{username}' " +
                    $"AND Password = '{password}'";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customerId = reader["CustomerId"] as string;
                    }
                }
            }
            return customerId;
        }

        public static Customer GetCustomerById(string customerId)
        {
            Customer customer = null;
            using (var conn = new MySqlConnection(OnlineRetailersStoreContext.ConnectionString))
            {
                conn.OpenAsync();
                var sql = $"SELECT * " +
                    $"FROM online_retailers_store.customer " +
                    $"WHERE CustomerId = '{customerId}'";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customer = new Customer()
                        {
                            Id = reader["CustomerId"] as string,
                            FirstName = reader["FirstName"] as string,
                            LastName = reader["LastName"] as string,
                            Username = reader["Username"] as string,
                            Password = reader["Password"] as string
                        };
                    }
                }
            }

            return customer;
        }

        public static CustomerInformation GetCustomerInformationById(string customerId)
        {
            CustomerInformation customer = null;

            using (var conn = new MySqlConnection(OnlineRetailersStoreContext.ConnectionString))
            {
                conn.OpenAsync();
                var sql = $"SELECT * " +
                    $"FROM online_retailers_store.customer " +
                    $"RIGHT JOIN online_retailers_store.address ON Address.CustomerId = Customer.CustomerId " +
                    $"RIGHT JOIN online_retailers_store.bank_account ON bank_account.CustomerId = Customer.CustomerId " +
                    $"WHERE customer.CustomerId = '{customerId}'";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customer = new CustomerInformation()
                        {
                            Customer = new Customer()
                            {
                                Id = reader["CustomerId"] as string,
                                FirstName = reader["FirstName"] as string,
                                LastName = reader["LastName"] as string,
                                Username = reader["Username"] as string,
                                Password = reader["Password"] as string,
                            },

                            Address = new Address()
                            {
                                Id = reader["AddressId"] as string,
                                Address1 = reader["Address1"] as string,
                                Address2 = reader["Address2"] as string,
                                City = reader["City"] as string,
                                State = reader["State"] as string,
                                ZipCode = (int)reader["ZipCode"],
                                Country = reader["Country"] as string,
                            },

                            BankAccount = new BankAccount()
                            {
                                Id = reader["BankAccountId"] as string,
                                Name = reader["Name"] as string,
                                CardNumber = reader["CardNumber"] as string,
                                ExpirationDate = (DateTime)reader["ExpirationDate"],
                                Cvc = (int)reader["Cvc"]
                            }
                        };
                    }
                }
            }
            return customer;
        }

        public static bool SaveCustomer(CustomerInformation customer)
        {
            using (var conn = new MySqlConnection(OnlineRetailersStoreContext.ConnectionString))
            {
                conn.OpenAsync();

                // If this is a new user, check if the username is taken
                if (string.IsNullOrEmpty(customer.Customer.Id))
                {

                    var sql = $"SELECT * " +
                        $"FROM online_retailers_store.customer " +
                        $"WHERE Username = '{customer.Customer.Username}' ";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);

                    using (var reader = cmd.ExecuteReader())
                    {
                        // If the reader can read this means the username provided is already taken
                        if (reader.Read())
                        {
                            return false;
                        }
                        else
                        {
                            reader.CloseAsync();
                            customer.Customer.Id = Guid.NewGuid().ToString();
                            customer.Address.Id = Guid.NewGuid().ToString();
                            customer.BankAccount.Id = Guid.NewGuid().ToString();

                            cmd = conn.CreateCommand();
                            cmd.CommandText = "INSERT INTO online_retailers_store.customer (CustomerId, Username, Password, FirstName, LastName) " +
                                "VALUES(@CustomerId, @Username, @Password, @FirstName, @LastName)";
                            cmd.Parameters.AddWithValue("@CustomerId", customer.Customer.Id);
                            cmd.Parameters.AddWithValue("@Username", customer.Customer.Username);
                            cmd.Parameters.AddWithValue("@Password", customer.Customer.Password);
                            cmd.Parameters.AddWithValue("@FirstName", customer.Customer.FirstName);
                            cmd.Parameters.AddWithValue("@LastName", customer.Customer.LastName);
                            cmd.ExecuteNonQuery();

                            cmd = conn.CreateCommand();
                            cmd.CommandText = "INSERT INTO online_retailers_store.address (AddressId, Address1, Address2, City, State, ZipCode, Country, CustomerId) " +
                                                "VALUES(@AddressId, @Address1, @Address2, @City, @State, @ZipCode, @Country, @CustomerId)";
                            cmd.Parameters.AddWithValue("@AddressId", customer.Address.Id);
                            cmd.Parameters.AddWithValue("@Address1", customer.Address.Address1);
                            cmd.Parameters.AddWithValue("@Address2", customer.Address.Address2);
                            cmd.Parameters.AddWithValue("@City", customer.Address.City);
                            cmd.Parameters.AddWithValue("@State", customer.Address.State);
                            cmd.Parameters.AddWithValue("@ZipCode", customer.Address.ZipCode);
                            cmd.Parameters.AddWithValue("@Country", customer.Address.Country);
                            cmd.Parameters.AddWithValue("@CustomerId", customer.Customer.Id);
                            cmd.ExecuteNonQuery();

                            cmd = conn.CreateCommand();
                            cmd.CommandText = "INSERT INTO online_retailers_store.bank_account (BankAccountId, Name, CardNumber, ExpirationDate, Cvc, CustomerId) " +
                                                "VALUES(@BankAccountId, @Name, @CardNumber, @ExpirationDate, @Cvc, @CustomerId)";
                            cmd.Parameters.AddWithValue("@BankAccountId", customer.BankAccount.Id);
                            cmd.Parameters.AddWithValue("@Name", customer.BankAccount.Name);
                            cmd.Parameters.AddWithValue("@CardNumber", customer.BankAccount.CardNumber);
                            cmd.Parameters.AddWithValue("@ExpirationDate", customer.BankAccount.ExpirationDate);
                            cmd.Parameters.AddWithValue("@Cvc", customer.BankAccount.Cvc);
                            cmd.Parameters.AddWithValue("@CustomerId", customer.Customer.Id);
                            cmd.ExecuteNonQuery();

                            return true;
                        }
                    }
                }

                var comm = conn.CreateCommand();
                comm.CommandText = "UPDATE online_retailers_store.customer " +
                    "SET Username = @Username, " +
                    "Password = @Password, " +
                    "FirstName = @FirstName, " +
                    "LastName = @LastName " +
                    $"WHERE CustomerId = '{customer.Customer.Id}'";
                comm.Parameters.AddWithValue("@Username", customer.Customer.Username);
                comm.Parameters.AddWithValue("@Password", customer.Customer.Password);
                comm.Parameters.AddWithValue("@FirstName", customer.Customer.FirstName);
                comm.Parameters.AddWithValue("@LastName", customer.Customer.LastName);
                comm.ExecuteNonQuery();

                comm = conn.CreateCommand();
                comm.CommandText = "UPDATE online_retailers_store.address " +
                                    "SET Address1 = @Address1, Address2 = @Address2, City = @City, State = @State, ZipCode = @ZipCode, Country = @Country " +
                                    $"WHERE CustomerId = '{customer.Customer.Id}'";
                comm.Parameters.AddWithValue("@Address1", customer.Address.Address1);
                comm.Parameters.AddWithValue("@Address2", customer.Address.Address2);
                comm.Parameters.AddWithValue("@City", customer.Address.City);
                comm.Parameters.AddWithValue("@State", customer.Address.State);
                comm.Parameters.AddWithValue("@ZipCode", customer.Address.ZipCode);
                comm.Parameters.AddWithValue("@Country", customer.Address.Country);
                comm.ExecuteNonQuery();

                comm = conn.CreateCommand();
                comm.CommandText = "UPDATE online_retailers_store.bank_account " +
                                    "SET Name = @Name, CardNumber = @CardNumber, ExpirationDate = @ExpirationDate, Cvc = @Cvc " +
                                    $"WHERE CustomerId = '{customer.Customer.Id}'";
                comm.Parameters.AddWithValue("@Name", customer.BankAccount.Name);
                comm.Parameters.AddWithValue("@CardNumber", customer.BankAccount.CardNumber);
                comm.Parameters.AddWithValue("@ExpirationDate", customer.BankAccount.ExpirationDate);
                comm.Parameters.AddWithValue("@Cvc", customer.BankAccount.Cvc);
                comm.ExecuteNonQuery();
            }
            return true;
        }
    }
}
