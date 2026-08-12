using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ECommerce.Models
{
    public class UserProducts
    {
        public int id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }
        public int PurchasePrice { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime LastPurchaseDate { get; set; }
    }
}
