using ECommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Data
{
    public class AuthContext (DbContextOptions<AuthContext> options) : DbContext(options)
    {
        public DbSet<UserModel> User => Set<UserModel>();
    }
}
