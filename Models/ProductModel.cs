namespace ECommerce.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public int StockRemaining { get; set; }
    }
}
