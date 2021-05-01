using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineRetailersStore.Data
{
    public static class SupplierService
    {
        public static string GetSupplierNameByUserId(string userId)
        {
            var supplierName = string.Empty;

            using (var conn = new MySqlConnection(OnlineRetailersStoreContext.ConnectionString))
            {   
                conn.OpenAsync();
                var sql = $"SELECT Name " +
                    $"FROM online_retailers_store.supplier " +
                    $"WHERE UserId = '{userId}'";

                var cmd = new MySqlCommand(sql, conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        supplierName = reader["Name"] as string;
                    }
                }
            }
            return supplierName;
        }
    }
}
