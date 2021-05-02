using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineRetailersStore.Data.Models
{
    public class Orders
    {
        public String OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Quantity { get; set; }
        public String Productid { get; set; }
        static public String CustomerId { get; set; }

        public static implicit operator List<object>(Orders v)
        {
            throw new NotImplementedException();
        }
    }
}
