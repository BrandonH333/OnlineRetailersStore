using System.Collections.Generic;

namespace OnlineRetailersStore.Data.Models
{
    public class ProductList
    {
        public List<Product> list { get; set; } = new List<Product>();
        public int size { get; set; }
    }
}
