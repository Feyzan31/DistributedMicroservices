using Microsoft.EntityFrameworkCore;
using ProductService_SQL.Models;

namespace ProductService_SQL.Data
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Product>()
              .ToTable("products")
              .HasKey(p => p.Id);

            mb.Entity<Product>()
              .Property(p => p.Name)
              .IsRequired()
              .HasMaxLength(200);

            mb.Entity<Product>()
              .Property(p => p.Price)
              .HasColumnType("decimal(18,2)");
        }
    }
}
