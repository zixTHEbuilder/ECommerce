using ECommerce.Models;

namespace ECommerce.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public int StockRemaining { get; set; }
        public ProductDto(ProductModel p)
        {
            Id = p.Id;
            ProductName = p.ProductName;
            ProductDescription = p.ProductDescription;
            Category = p.Category;
            Price = p.Price;
            StockRemaining = p.StockRemaining;
        }
    }
}
