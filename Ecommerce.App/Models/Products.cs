using System.ComponentModel.DataAnnotations;

namespace Ecommerce.App.Models
{
    public class Product
    {
        public int Id { get; set; }
        
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
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsActive { get; set; } = true;
        
        // Foreign Keys
        [Required]
        public string SellerId { get; set; } = string.Empty;
        
        [Required]
        public int CategoryId { get; set; }
        
        // Navigation Properties
        public ApplicationUser Seller { get; set; } = null!;
        public Category Category { get; set; } = null!;
    }
    
    public class Category
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? Description { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Foreign Key
        [Required]
        public string SellerId { get; set; } = string.Empty;
        
        // Navigation Properties
        public ApplicationUser Seller { get; set; } = null!;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}