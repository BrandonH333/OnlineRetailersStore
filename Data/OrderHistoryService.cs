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
        public static List<OrderHistory> OrdersByUserId(string userId)
        {
            List<OrderHistory> orders = new List<OrderHistory>();
            using (var conn = new MySqlConnection(OnlineRetailersStoreContext.ConnectionString))
            {
                conn.OpenAsync();
                var sql = $"SELECT * " +
                    $"FROM online_retailers_store.order " +
                    $"JOIN online_retailers_store.product ON product.ProductId = order.ProductId " +
                    $"WHERE UserId = '{userId}' " +
                    $"ORDER BY CreatedDate DESC, LineItemId";

                var cmd = new MySqlCommand(sql, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new OrderHistory()
                        {
                            UserId = reader["UserId"] as string,
                            ProductId = reader["ProductId"] as string,
                            Quantity = (int)(reader["Quantity"] as int?),
                            Price = (decimal)(reader["Price"] as decimal?),
                            Name = reader["Name"] as string,
                            LineItemId = (int)(reader["LineItemId"] as int?),
                            CreatedDate = (DateTime)(reader["CreatedDate"] as DateTime?),
                            OrderNumber = reader["OrderNumber"] as string
                        });
                    }
                }
            }

            return orders;
        }

    }
}
