using MySql.Data.MySqlClient;
using OnlineRetailersStore.Data.Models;
using System;

namespace OnlineRetailersStore.Data
{
    public static class UserService
    {
        public static string GetUserIdByUsernameAndPassword(string username, string password)
        {
            string? userId = null;

            using (var conn = new MySqlConnection(OnlineRetailersStoreContext.ConnectionString))
            {
                conn.OpenAsync();
                var sql = $"SELECT * " +
                    $"FROM online_retailers_store.user " +
                    $"WHERE Username = '{username}' " +
                    $"AND Password = '{password}'";

                var cmd = new MySqlCommand(sql, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        userId = reader["UserId"] as string;
                    }
                }
            }
            return userId;
        }

        public static User GetUserById(string userId)
        {
            User user = null;
            using (var conn = new MySqlConnection(OnlineRetailersStoreContext.ConnectionString))
            {
                conn.OpenAsync();
                var sql = $"SELECT * " +
                    $"FROM online_retailers_store.user " +
                    $"WHERE UserId = '{userId}'";

                var cmd = new MySqlCommand(sql, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = new User()
                        {
                            Id = reader["UserId"] as string,
                            FirstName = reader["FirstName"] as string,
                            LastName = reader["LastName"] as string,
                            Username = reader["Username"] as string,
                            Password = reader["Password"] as string,
                            Retailer = (bool)reader["Retailer"]
                        };
                    }
                }
            }

            return user;
        }

        public static UserInformation GetUserInformationById(string userId)
        {
            UserInformation user = null;

            using (var conn = new MySqlConnection(OnlineRetailersStoreContext.ConnectionString))
            {
                conn.OpenAsync();
                var sql = $"SELECT * " +
                    $"FROM online_retailers_store.user " +
                    $"LEFT JOIN online_retailers_store.address ON Address.UserId = User.UserId " +
                    $"LEFT JOIN online_retailers_store.bank_account ON bank_account.UserId = User.UserId " +
                    $"WHERE user.UserId = '{userId}'";

                var cmd = new MySqlCommand(sql, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = new UserInformation()
                        {
                            User = new User()
                            {
                                Id = reader["UserId"] as string,
                                FirstName = reader["FirstName"] as string,
                                LastName = reader["LastName"] as string,
                                Username = reader["Username"] as string,
                                Password = reader["Password"] as string,
                                Retailer = (bool)reader["Retailer"]
                            }                            
                        };

                        if (!user.User.Retailer)
                        {

                            user.Address = new Address()
                            {
                                Id = reader["AddressId"] as string,
                                Address1 = reader["Address1"] as string,
                                Address2 = reader["Address2"] as string,
                                City = reader["City"] as string,
                                State = reader["State"] as string,
                                ZipCode = (int)reader["ZipCode"],
                                Country = reader["Country"] as string,
                            };

                            user.BankAccount = new BankAccount()
                            {
                                Id = reader["BankAccountId"] as string,
                                Name = reader["Name"] as string,
                                CardNumber = reader["CardNumber"] as string,
                                ExpirationDate = (DateTime)reader["ExpirationDate"],
                                Cvc = (int)reader["Cvc"]
                            };
                        }
                    }
                }
            }
            return user;
        }

        public static bool SaveUser(UserInformation user)
        {
            using (var conn = new MySqlConnection(OnlineRetailersStoreContext.ConnectionString))
            {
                conn.OpenAsync();

                // If this is a new user, check if the username is taken
                if (string.IsNullOrEmpty(user.User.Id))
                {

                    var sql = $"SELECT * " +
                        $"FROM online_retailers_store.user " +
                        $"WHERE Username = '{user.User.Username}' ";

                    var cmd = new MySqlCommand(sql, conn);

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
                            user.User.Id = Guid.NewGuid().ToString();
                            user.Address.Id = Guid.NewGuid().ToString();
                            user.BankAccount.Id = Guid.NewGuid().ToString();

                            cmd = conn.CreateCommand();
                            cmd.CommandText = "INSERT INTO online_retailers_store.user (UserId, Username, Password, FirstName, LastName) " +
                                "VALUES(@UserId, @Username, @Password, @FirstName, @LastName)";
                            cmd.Parameters.AddWithValue("@UserId", user.User.Id);
                            cmd.Parameters.AddWithValue("@Username", user.User.Username);
                            cmd.Parameters.AddWithValue("@Password", user.User.Password);
                            cmd.Parameters.AddWithValue("@FirstName", user.User.FirstName);
                            cmd.Parameters.AddWithValue("@LastName", user.User.LastName);
                            cmd.ExecuteNonQuery();

                            if(user.Address != null)
                            {
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "INSERT INTO online_retailers_store.address (AddressId, Address1, Address2, City, State, ZipCode, Country, UserId) " +
                                                    "VALUES(@AddressId, @Address1, @Address2, @City, @State, @ZipCode, @Country, @UserId)";
                                cmd.Parameters.AddWithValue("@AddressId", user.Address.Id);
                                cmd.Parameters.AddWithValue("@Address1", user.Address.Address1);
                                cmd.Parameters.AddWithValue("@Address2", user.Address.Address2);
                                cmd.Parameters.AddWithValue("@City", user.Address.City);
                                cmd.Parameters.AddWithValue("@State", user.Address.State);
                                cmd.Parameters.AddWithValue("@ZipCode", user.Address.ZipCode);
                                cmd.Parameters.AddWithValue("@Country", user.Address.Country);
                                cmd.Parameters.AddWithValue("@UserId", user.User.Id);
                                cmd.ExecuteNonQuery();
                            }
                            
                            if(user.BankAccount != null)
                            {
                                cmd = conn.CreateCommand();
                                cmd.CommandText = "INSERT INTO online_retailers_store.bank_account (BankAccountId, Name, CardNumber, ExpirationDate, Cvc, UserId) " +
                                                    "VALUES(@BankAccountId, @Name, @CardNumber, @ExpirationDate, @Cvc, @UserId)";
                                cmd.Parameters.AddWithValue("@BankAccountId", user.BankAccount.Id);
                                cmd.Parameters.AddWithValue("@Name", user.BankAccount.Name);
                                cmd.Parameters.AddWithValue("@CardNumber", user.BankAccount.CardNumber);
                                cmd.Parameters.AddWithValue("@ExpirationDate", user.BankAccount.ExpirationDate);
                                cmd.Parameters.AddWithValue("@Cvc", user.BankAccount.Cvc);
                                cmd.Parameters.AddWithValue("@UserId", user.User.Id);
                                cmd.ExecuteNonQuery();
                            }

                            return true;
                        }
                    }
                }
                else
                {
                    // Update existing user
                    var comm = conn.CreateCommand();
                    comm.CommandText = "UPDATE online_retailers_store.user " +
                        "SET Username = @Username, " +
                        "Password = @Password, " +
                        "FirstName = @FirstName, " +
                        "LastName = @LastName " +
                        $"WHERE UserId = '{user.User.Id}'";
                    comm.Parameters.AddWithValue("@Username", user.User.Username);
                    comm.Parameters.AddWithValue("@Password", user.User.Password);
                    comm.Parameters.AddWithValue("@FirstName", user.User.FirstName);
                    comm.Parameters.AddWithValue("@LastName", user.User.LastName);
                    comm.ExecuteNonQuery();

                    if (!user.User.Retailer)
                    {
                        comm = conn.CreateCommand();
                        comm.CommandText = "UPDATE online_retailers_store.address " +
                                            "SET Address1 = @Address1, Address2 = @Address2, City = @City, State = @State, ZipCode = @ZipCode, Country = @Country " +
                                            $"WHERE UserId = '{user.User.Id}'";
                        comm.Parameters.AddWithValue("@Address1", user.Address.Address1);
                        comm.Parameters.AddWithValue("@Address2", user.Address.Address2);
                        comm.Parameters.AddWithValue("@City", user.Address.City);
                        comm.Parameters.AddWithValue("@State", user.Address.State);
                        comm.Parameters.AddWithValue("@ZipCode", user.Address.ZipCode);
                        comm.Parameters.AddWithValue("@Country", user.Address.Country);
                        comm.ExecuteNonQuery();

                        comm = conn.CreateCommand();
                        comm.CommandText = "UPDATE online_retailers_store.bank_account " +
                                            "SET Name = @Name, CardNumber = @CardNumber, ExpirationDate = @ExpirationDate, Cvc = @Cvc " +
                                            $"WHERE UserId = '{user.User.Id}'";
                        comm.Parameters.AddWithValue("@Name", user.BankAccount.Name);
                        comm.Parameters.AddWithValue("@CardNumber", user.BankAccount.CardNumber);
                        comm.Parameters.AddWithValue("@ExpirationDate", user.BankAccount.ExpirationDate);
                        comm.Parameters.AddWithValue("@Cvc", user.BankAccount.Cvc);
                        comm.ExecuteNonQuery();
                    }
                    
                }
            }
            return true;
        }
    }
}
