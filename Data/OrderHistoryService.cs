using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineRetailersStore.Data.Models;

namespace OnlineRetailersStore.Data
{
    public class OrderHistoryService
    {
        public static List<OrderHistory> OrdersByCustomerId(string customerId)
        {
            List<OrderHistory> orders = new List<OrderHistory>();
            using (var conn = new MySqlConnection(OnlineRetailersStoreContext.ConnectionString))
            {
                conn.OpenAsync();
                var sql = $"SELECT * " +
                    $"FROM online_retailers_store.order_history " +
                    $"WHERE CustomerId = '{customerId}'";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new OrderHistory()
                        {
                            CustomerId = reader["CustomerId"] as string,
                            ProductId = reader["Product_Id"] as string,
                            Quantity = (int)(reader["Quantity"] as int?),
                            Price = (decimal)(reader["Price"] as decimal?),
                            Name = reader["Name"] as string,
                            OrderId = reader["OrderId"] as string
                        });
                    }
                }
            }

            return orders;
        }

    }
}
