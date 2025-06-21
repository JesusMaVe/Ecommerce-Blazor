using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ecommerce.App.Models;

namespace Ecommerce.App.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ApplicationUser configuration
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.FirstName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(50).IsRequired();
                entity.Property(e => e.UserType).HasConversion<int>();
            });

            // Product configuration
            builder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500).IsRequired();
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.ImageUrl).HasMaxLength(500);
                
                // Relationship with Seller
                entity.HasOne(e => e.Seller)
                    .WithMany()
                    .HasForeignKey(e => e.SellerId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                // Relationship with Category
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Products)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Category configuration
            builder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(200);
                
                // Relationship with Seller
                entity.HasOne(e => e.Seller)
                    .WithMany()
                    .HasForeignKey(e => e.SellerId)
                    .OnDelete(DeleteBehavior.Cascade);
                
                // Unique constraint for seller-category name
                entity.HasIndex(e => new { e.SellerId, e.Name })
                    .IsUnique();
            });
        }
    }
}