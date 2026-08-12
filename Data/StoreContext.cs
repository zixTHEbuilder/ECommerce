using ECommerce.Dtos;
using ECommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data
{
    public class StoreContext (DbContextOptions<StoreContext> options) : DbContext(options)
    {
        public DbSet<ProductModel> Products => Set<ProductModel>();
        public DbSet<UserProducts> UserProducts => Set<UserProducts>();
        public DbSet<Cart> Cart => Set<Cart>();
    }
}
