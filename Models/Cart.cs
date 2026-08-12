namespace ECommerce.Models
{
    public class Cart
    {
        public int id { get; set; }
        public int userId { get; set; }
        public int PurchasePrice { get; set; }
        public int ProductId { get; set;}
        public int ProductQuantity { get; set; }
        DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
