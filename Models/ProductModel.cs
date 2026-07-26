namespace ECommerce.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public required string ProductName { get; set; }
        public required string ProductDescription { get; set; }
        public required string Category { get; set; }
        public required int Price { get; set; }
        public int StockRemaining { get; set; }
    }
}
