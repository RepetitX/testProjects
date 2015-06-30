using System.Data.Entity;
using IShop.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IShop.DataAccess.Context
{
    public class IShopContext : IdentityDbContext<ApplicationUser>
    {
        public IShopContext()
            : base("IShopContext", false)
        {
        }

        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
            .HasOptional(e => e.UserManager)
            .WithMany(e => e.Orders)
            .HasForeignKey(e => e.UserManagerId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<ProductOption>()
            .HasRequired(e => e.ProductType)
            .WithMany(e => e.ProductOptions)
            .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}