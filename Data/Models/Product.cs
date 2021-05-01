namespace OnlineRetailersStore.Data.Models
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string SupplierId { get; set; }
        public string ImageSrc { get; set; }
        public int Inventory { get; set; }
    }
}
