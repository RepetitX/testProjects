using System.Data.Entity;
using Core.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public IDbSet<Product> Products { get; set; }
        public IDbSet<Manager> Managers { get; set; }
        public IDbSet<Order> Orders { get; set; }
        public IDbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //настроим композитный ключ через fluentApi
            modelBuilder
                .Entity<OrderItem>()
                .HasKey(t => new { t.OrderId, t.ProductId });

            //отношение Many2Many
            modelBuilder.Entity<Product>()
                  .HasMany(s => s.Accessorises)
                  .WithMany()
                  .Map(cs =>
                  {
                      cs.MapLeftKey("MainProductId");
                      cs.MapRightKey("AccessoryProductId");
                      cs.ToTable("ProductRelations");
                  });
            //
            base.OnModelCreating(modelBuilder);
        }
    }
}