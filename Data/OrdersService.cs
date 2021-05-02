using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using OnlineRetailersStore.Data.Models;

namespace OnlineRetailersStore.Data
{
    public class OrdersService
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

        public static List<Orders> OrdersByCustomerId(string customerId)
        {
            List<Orders> orders = new List<Orders>();
            using (var conn = new MySqlConnection(OnlineRetailersStoreContext.ConnectionString))
            {
                conn.OpenAsync();
                var sql = $"SELECT CustomerId " +
                    $"FROM online_retailers_store.order " +
                    $"WHERE CustomerId = '{customerId}'";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new Orders()
                        {
                            OrderId = reader["OrderId"] as string,
                            Productid = reader["ProductId"] as string,
                        });
                    }
                }
            }

            return orders;
        }
    }
}