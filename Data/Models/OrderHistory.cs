using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineRetailersStore.Data.Models
{
    public class OrderHistory
    {
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string OrderNumber { get; set; }
        public int LineItemId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
