using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Domain.Entites;

namespace RestaurantManagementSystem.Infrastructure.Context
{
    public class RestaurantDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Category
            modelBuilder.Entity<Category>(builder =>
            {
                builder.ToTable("Categories");
                builder.HasKey(c => c.Id);
                builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
                builder.HasMany(c => c.MenuItems)
                       .WithOne(m => m.Category)
                       .HasForeignKey(m => m.CategoryId)
                       .OnDelete(DeleteBehavior.Restrict);
            });

            // MenuItem
            modelBuilder.Entity<MenuItem>(builder =>
            {
                builder.ToTable("MenuItems");
                builder.HasKey(m => m.Id);
                builder.Property(m => m.Name).IsRequired().HasMaxLength(100);
                builder.Property(m => m.Price).HasColumnType("decimal(18,2)");
                builder.Property(m => m.IsAvailable).HasDefaultValue(true);
                builder.Property(m => m.PreparationTime).IsRequired();
                builder.Property(m => m.RowVersion).IsRowVersion();
                builder.HasIndex(m => m.CategoryId);
            });

            // Order
            modelBuilder.Entity<Order>(builder =>
            {
                builder.ToTable("Orders");
                builder.HasKey(o => o.Id);
                builder.Property(o => o.CustomerId).IsRequired();
                builder.Property(o => o.OrderType).IsRequired();
                builder.Property(o => o.Status).IsRequired();
                builder.Property(o => o.DeliveryAddress).HasMaxLength(200);
                builder.Property(o => o.Total).HasColumnType("decimal(18,2)");
                builder.Property(o => o.OrderDate).HasDefaultValueSql("GETUTCDATE()");
                builder.HasMany(o => o.OrderItems)
                       .WithOne(oi => oi.Order)
                       .HasForeignKey(oi => oi.OrderId)
                       .OnDelete(DeleteBehavior.Cascade);
            });

            // OrderItem
            modelBuilder.Entity<OrderItem>(builder =>
            {
                builder.ToTable("OrderItems");
                builder.HasKey(oi => new { oi.OrderId, oi.MenuItemId });
                builder.Property(oi => oi.Quantity).IsRequired();
                builder.Property(oi => oi.Subtotal).HasColumnType("decimal(18,2)");
                builder.HasOne(oi => oi.Order)
                       .WithMany(o => o.OrderItems)
                       .HasForeignKey(oi => oi.OrderId);
                builder.HasOne(oi => oi.MenuItem)
                       .WithMany(m => m.OrderItems)
                       .HasForeignKey(oi => oi.MenuItemId)
                       .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}