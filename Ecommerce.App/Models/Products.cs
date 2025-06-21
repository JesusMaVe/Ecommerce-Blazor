using System.ComponentModel.DataAnnotations;

namespace Ecommerce.App.Models
{
    public class Product
    {
        public int Id { get; init; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal Price { get; set; }
        
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser mayor o igual a 0")]
        public int Stock { get; set; }
        
        public string? ImageUrl { get; set; }
        
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        
        public bool IsActive { get; set; } = true;
        
        // Foreign Keys
        [Required]
        public string SellerId { get; init; } = string.Empty;
        
        [Required]
        public int CategoryId { get; set; }
        
        // Navigation Properties
        public ApplicationUser Seller { get; init; } = null!;
        public Category Category { get; init; } = null!;
    }
    
    public class Category
    {
        public int Id { get; init; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; init; } = string.Empty;
        
        [StringLength(200)]
        public string? Description { get; init; }
        
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        
        // Foreign Key
        [Required]
        public string SellerId { get; init; } = string.Empty;
        
        // Navigation Properties
        public ApplicationUser Seller { get; init; } = null!;
        public ICollection<Product> Products { get; init; } = new List<Product>();
    }
}