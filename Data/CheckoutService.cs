using MySql.Data.MySqlClient;
using OnlineRetailersStore.Data.Models;
using System;
using System.Collections.Generic;

namespace OnlineRetailersStore.Data
{
    public class CheckoutService
    {
        public static bool PlaceOrder(List<ShoppingCart> order)
        {
            using (var conn = new MySqlConnection(OnlineRetailersStoreContext.ConnectionString))
            {
                conn.OpenAsync();
                
                var lineItemId = 1;
                var orderNumber = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
                foreach (var lineItem in order)
                {
                    var comm = conn.CreateCommand();

                    comm.CommandText = "INSERT INTO online_retailers_store.order " +
                        "SET OrderId = @OrderId, " +
                        "OrderNumber = @OrderNumber, " +
                        "CreatedDate = @CreatedDate, " +
                        "LineItemId = @LineItemId, " +
                        "Quantity = @Quantity, " +
                        "Price = @Price, " +
                        "ProductId = @ProductId, " +
                        "UserId = @UserId ";

                    comm.Parameters.AddWithValue("@OrderId", Guid.NewGuid().ToString());
                    comm.Parameters.AddWithValue("@OrderNumber", orderNumber);
                    comm.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    comm.Parameters.AddWithValue("@LineItemId", lineItemId);
                    comm.Parameters.AddWithValue("@Quantity", lineItem.Quantity);
                    comm.Parameters.AddWithValue("@Price", lineItem.ProductPrice/lineItem.Quantity);
                    comm.Parameters.AddWithValue("@ProductId", lineItem.ProductId);
                    comm.Parameters.AddWithValue("@UserId", lineItem.UserId);
                    comm.ExecuteNonQuery();

                    lineItemId++;
                }

                var comm2 = conn.CreateCommand();
                comm2.CommandText = "DELETE FROM online_retailers_store.shopping_cart " +
                    $"WHERE shopping_cart.ShoppingCartId = '{order[0].Id}'";

                comm2.ExecuteNonQuery();
            }

            return true;
        }
    }
}
