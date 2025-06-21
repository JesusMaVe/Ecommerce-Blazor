using System.ComponentModel.DataAnnotations;

namespace Ecommerce.App.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; init; } = string.Empty;
        
        [Required]
        public int ProductId { get; init; }
        
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Quantity { get; set; } = 1;
        
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        
        // Navigation Properties
        public ApplicationUser User { get; init; } = null!;
        public Product Product { get; init; } = null!;
    }
    
    public class WishlistItem
    {
        public int Id { get; init; }
        
        [Required]
        public string UserId { get; init; } = string.Empty;
        
        [Required]
        public int ProductId { get; init; }
        
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        
        // Navigation Properties
        public ApplicationUser User { get; init; } = null!;
        public Product Product { get; init; } = null!;
    }
}