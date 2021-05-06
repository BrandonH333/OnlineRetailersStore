using System.Collections.Generic;

namespace OnlineRetailersStore.Data.Models
{
    public class ShoppingCartList
    {
        public List<ShoppingCart> list { get; set; } = new List<ShoppingCart>();
        public int size { get; set; }
    }
}
