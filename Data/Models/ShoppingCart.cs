namespace OnlineRetailersStore.Data.Models
{
    public class ShoppingCart
    {
        public string Id { get; set; }
        public string LineItemId { get; set; }
        public int Quantity { get; set; }
        public string ProductId { get; set; }
        public string UserId { get; set; }
    }
}
