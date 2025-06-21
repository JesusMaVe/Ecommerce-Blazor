using System.ComponentModel.DataAnnotations;

namespace Ecommerce.App.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Quantity { get; set; } = 1;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation Properties
        public ApplicationUser User { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
    
    public class WishlistItem
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [Required]
        public int ProductId { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation Properties
        public ApplicationUser User { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}