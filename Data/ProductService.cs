using MySql.Data.MySqlClient;
using OnlineRetailersStore.Data.Models;
using System;
using System.Collections.Generic;

namespace OnlineRetailersStore.Data
{
    public static class ProductService
    {
        public static Product GetProductById(string productId)
        {
            Product product = null;
            using (var conn = new MySqlConnection(OnlineRetailersStoreContext.ConnectionString))
            {
                conn.OpenAsync();
                var sql = $"SELECT * " +
                    $"FROM online_retailers_store.product " +
                    $"WHERE ProductId = '{productId}'";

                var cmd = new MySqlCommand(sql, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        product = new Product()
                        {
                            Id = reader["ProductId"] as string,
                            Name = reader["Name"] as string,
                            Description = reader["Description"] as string,
                            Price = (int)reader["Price"],
                            SupplierId = reader["SupplierId"] as string
                        };
                    }
                }
            }

            return product;
        }

        public static List<Product> GetProductList()
        {
            var productList = new List<Product>();

            using (var conn = new MySqlConnection(OnlineRetailersStoreContext.ConnectionString))
            {
                conn.OpenAsync();
                var sql = $"SELECT * " +
                    $"FROM online_retailers_store.product ";

                var cmd = new MySqlCommand(sql, conn);

                using (var data_reader = cmd.ExecuteReader())
                {
                    if (data_reader.HasRows)
                    {
                        while (data_reader.Read())
                        {
                            Product product = new Product
                            {
                                Id = data_reader["ProductId"] as string,
                                Name = data_reader["Name"] as string,
                                Description = data_reader["Description"] as string,
                                Price = (decimal)data_reader["Price"],
                                ImageSrc = data_reader["ImageSrc"] as string,
                                Inventory = (int)data_reader["Inventory"],
                                SupplierId = data_reader["SupplierId"] as string,
                            };

                            productList.Add(product);
                        }
                    }
                }
            }
            return productList;
        }

        public static List<Product> GetSupplierProductsByUserId(string userId)
        {
            var productList = new List<Product>();
            using (var conn = new MySqlConnection(OnlineRetailersStoreContext.ConnectionString))
            {
                conn.OpenAsync();
                var sql = $"SELECT product.* " +
                    $"FROM online_retailers_store.user " +
                    $"JOIN online_retailers_store.supplier ON supplier.UserId = user.userId " +
                    $"JOIN online_retailers_store.product ON product.SupplierId = supplier.SupplierId " +
                    $"WHERE User.UserId = '{userId}' ";

                var cmd = new MySqlCommand(sql, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product product = new Product
                        {
                            Id = reader["ProductId"] as string,
                            Name = reader["Name"] as string,
                            Description = reader["Description"] as string,
                            Price = (decimal)reader["Price"],
                            ImageSrc = reader["ImageSrc"] as string,
                            Inventory = (int)reader["Inventory"],
                            SupplierId = reader["SupplierId"] as string,
                        };

                        productList.Add(product);
                    }
                }
            }
            return productList;
        }

        public static bool SaveProducts(List<Product> products)
        {
            using (var conn = new MySqlConnection(OnlineRetailersStoreContext.ConnectionString))
            {
                conn.OpenAsync();
                var cmd = conn.CreateCommand();
                foreach (var product in products)
                {
                    if(product.Id == null)
                    {
                        product.Id = Guid.NewGuid().ToString();
                        cmd.CommandText = "INSERT INTO online_retailers_store.product (ProductId, Name, Description, Price, ImageSrc, Inventory, SupplierId) " +
                                $"VALUES('{product.Id}', '{product.Name}', '{product.Description}', {product.Price}, '{product.ImageSrc}', {product.Inventory}, '{product.SupplierId}')";
                    }
                    else
                    {
                        cmd.CommandText = $"UPDATE online_retailers_store.product " +
                            $"SET Name = '{product.Name}', Description = '{product.Description}', Price = '{product.Price}', ImageSrc = '{product.ImageSrc}', Inventory = {product.Inventory} " +
                            $"WHERE ProductId = '{product.Id}'";
                    }

                    cmd.ExecuteNonQuery();
                }
            }

            return true;
        }
    }
}
