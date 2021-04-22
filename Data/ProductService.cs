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

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        product = new Product()
                        {
                            Id = reader["ProductId"] as string,
                            Name = reader["Name"] as string,
                            Description = reader["Description"] as string,
                            Price = reader["Price"] as string,
                            SupplierId = reader["SupplierId"] as string
                        };
                    }
                }
            }

            return product;
        }


        public static ProductList GetProductList()
        {
            ProductList productList = new ProductList();
            
            using (var conn = new MySqlConnection(OnlineRetailersStoreContext.ConnectionString))
            {
                conn.OpenAsync();
                var sql = $"SELECT * " +
                    $"FROM online_retailers_store.product ";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                using (var data_reader = cmd.ExecuteReader())
                {
                    if (data_reader.HasRows)
                    {
                        int count = data_reader.FieldCount;
                        int columnSize = 0;
                        while (data_reader.Read())
                        {
                            for (var i = 0; i < count; i += 6)
                            {

                                //Debug.WriteLine(data_reader.GetValue(i));
                                Product product = new Product();
                                product.Id = data_reader.GetValue(i).ToString();
                                product.Name = data_reader.GetValue(i+1).ToString();
                                product.Description = data_reader.GetValue(i + 2).ToString();
                                product.Price = data_reader.GetValue(i + 3).ToString();
                                product.SupplierId = data_reader.GetValue(i + 4).ToString();
                                product.imageSrc = data_reader.GetValue(i + 5).ToString();

                                productList.list.Add(product);
                                columnSize++;
                            }
                        }
                        productList.size = columnSize;
                        data_reader.Close();
                    }
                }

            }
            return productList;
        }
    }
}
