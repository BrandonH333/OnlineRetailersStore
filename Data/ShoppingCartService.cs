using MySql.Data.MySqlClient;
using OnlineRetailersStore.Data.Models;
using System;
using System.Collections.Generic;

namespace OnlineRetailersStore.Data
{
    public static class ShoppingCartService
    {
        

        public static List<ShoppingCart> GetShoppingCartList(string UserId)
        {
            var shoppingCartList = new List<ShoppingCart>();

            using (var conn = new MySqlConnection(OnlineRetailersStoreContext.ConnectionString))
            {
                conn.OpenAsync();
                var sql = $"SELECT * " +
                    $"FROM online_retailers_store.shopping_cart " +
                    $"WHERE UserId = '{UserId}' ";

                var cmd = new MySqlCommand(sql, conn);

                using (var data_reader = cmd.ExecuteReader())
                {
                    if (data_reader.HasRows)
                    {
                        while (data_reader.Read())
                        {
                            ShoppingCart product = new ShoppingCart
                            {
                                Id = data_reader["ShoppingCartId"] as string,
                                LineItemId = data_reader["LineItemId"] as string,
                                Quantity = (int)data_reader["Quantity"],
                                ProductId = data_reader["ProductId"] as string,
                                UserId = data_reader["UserId"] as string,

                            };

                            shoppingCartList.Add(product);
                        }
                    }
                }
            }
            return shoppingCartList;
        }

        public static bool UpdateQuantityForCart(string productId, int quantity, string userId)
        {
            using (var conn = new MySqlConnection(OnlineRetailersStoreContext.ConnectionString))
            {
                conn.OpenAsync();
                var cmd = conn.CreateCommand();
                cmd.CommandText = $"UPDATE online_retailers_store.shopping_cart " +
                            $"SET Quantity = '{quantity}' " +
                            $"WHERE ProductId = '{productId}' and UserId = '{userId}' ";
                cmd.ExecuteNonQuery();
            }
            return true;
        }

        public static bool TakeAwayQuantityForProduct(int currentInventory, string productId, int quantity, string userId)
        {
            using (var conn = new MySqlConnection(OnlineRetailersStoreContext.ConnectionString))
            {
                conn.OpenAsync();
                var cmd = conn.CreateCommand();

                cmd.CommandText = $"UPDATE online_retailers_store.product " +
                            $"SET Inventory = '{currentInventory - 1}' " +
                            $"WHERE ProductId = '{productId}' ";
                cmd.ExecuteNonQuery();
            }
            return true;
        }

        public static bool AddQuantityForProduct(int currentInventory, string productId, int quantity, string userId)
        {
            using (var conn = new MySqlConnection(OnlineRetailersStoreContext.ConnectionString))
            {
                conn.OpenAsync();
                var cmd = conn.CreateCommand();

                cmd.CommandText = $"UPDATE online_retailers_store.product " +
                            $"SET Inventory = '{currentInventory + 1}' " +
                            $"WHERE ProductId = '{productId}' ";
                cmd.ExecuteNonQuery();
            }
            return true;
        }


    }
}
