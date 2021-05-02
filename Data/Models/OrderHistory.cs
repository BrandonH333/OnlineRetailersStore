using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineRetailersStore.Data.Models
{
    public class OrderHistory
    {
        public String UserId { get; set; }
        public String ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public String Name { get; set; }
        public String OrderId { get; set; }
        public int LineItemId { get; set; }
        public DateTime CreatedDate { get; set; }

        ICollection<Orders> Orders { get; set; }
    }
}
