using Microsoft.EntityFrameworkCore;
using OrderService_SQL.Models;

namespace OrderService_SQL.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            mb.Entity<Order>()
              .ToTable("orders")
              .HasKey(o => o.Id);

            mb.Entity<Order>()
              .Property(o => o.ProductId)
              .IsRequired();

            mb.Entity<Order>()
              .Property(o => o.Quantity)
              .IsRequired();

            mb.Entity<Order>()
              .Property(o => o.OrderedAt)
              .IsRequired();
        }
    }
}