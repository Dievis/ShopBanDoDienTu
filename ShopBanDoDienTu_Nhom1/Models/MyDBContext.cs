using Microsoft.AspNet.Identity.EntityFramework;
using ShopBanDoDienTu_Nhom1.Identity;
using System.Data.Entity;

namespace ShopBanDoDienTu_Nhom1.Models
{
    public class MyDBContext : IdentityDbContext<AppUser>
    {
        public MyDBContext() : base("MyConnectionString") { }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

    }
}